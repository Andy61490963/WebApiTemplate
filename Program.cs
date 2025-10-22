using LoaderScheduler.Configuration;
using LoaderScheduler.Logging;
using LoaderScheduler.Services.Infrastructure;
using LoaderScheduler.Services.Interface;
using LoaderScheduler.Services.Processing;
using LoaderScheduler.Services.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LoaderScheduler;

/// <summary>
/// 程式進入點，設定主機與背景作業。
/// </summary>
/// <remarks>Remark: 以通用主機搭配 Serilog 與選項模式構建常駐排程程式。</remarks>
public static class Program
{
    /// <summary>
    /// 應用程式主要進入點。
    /// </summary>
    /// <param name="args">命令列參數。</param>
    public static async Task Main(string[] args)
    {
        Log.Logger = SerilogConfigurator.CreateBootstrapLogger();

        try
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Host.UseWindowsService();
            builder.Host.UseSystemd();
            ConfigureConfiguration(builder);
            ConfigureLogging(builder);
            ConfigureServices(builder);

            using var host = builder.Build();
            await host.RunAsync().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "應用程式啟動失敗。");
            throw;
        }
        finally
        {
            await Log.CloseAndFlushAsync().ConfigureAwait(false);
        }
    }

    private static void ConfigureConfiguration(HostApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    private static void ConfigureLogging(HostApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, _, loggerConfiguration) =>
        {
            SerilogConfigurator.Configure(loggerConfiguration, context.Configuration, context.HostingEnvironment);
        });
    }

    private static void ConfigureServices(HostApplicationBuilder builder)
    {
        RegisterOptions(builder);

        builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        builder.Services.AddSingleton<IDapperRepository, DapperRepository>();
        builder.Services.AddSingleton<IJob, ExampleJob>();
        builder.Services.AddHostedService<Worker>();
    }

    private static void RegisterOptions(HostApplicationBuilder builder)
    {
        builder.Services.AddOptions<SchedulerOptions>()
            .Bind(builder.Configuration.GetSection("Scheduler"))
            .ValidateDataAnnotations()
            .Validate(options => options.IntervalSeconds > 0, "IntervalSeconds 必須大於零。")
            .ValidateOnStart();

        builder.Services.AddOptions<DatabaseOptions>()
            .Configure(options => options.ConnectionString = GetRequiredConnectionString(builder.Configuration))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString), "ConnectionString 不得為空白。")
            .ValidateOnStart();

        builder.Services.AddOptions<LoggingOptions>()
            .Bind(builder.Configuration.GetSection("Logging"));
    }

    private static string GetRequiredConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("請在 appsettings.json 中設定 ConnectionStrings:DefaultConnection。");
        }

        return connectionString;
    }
}
