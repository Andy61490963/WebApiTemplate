using System.Reflection;
using System.Runtime.InteropServices;
using LoaderScheduler.Configuration;
using LoaderScheduler.Logging;
using LoaderScheduler.Services.Infrastructure;
using LoaderScheduler.Services.Interface;
using LoaderScheduler.Services.Processing;
using LoaderScheduler.Services.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace LoaderScheduler;

public static class Program
{
    private const string WindowsServiceName = "Loader Scheduler";

    public static async Task Main(string[] args)
    {
        Log.Logger = SerilogConfigurator.CreateBootstrapLogger();

        try
        {
            using var host = BuildHost(args);
            await host.RunAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
            Environment.ExitCode = 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHost BuildHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            // 設定檔：沿用預設（appsettings.json / appsettings.{Env}.json / 環境變數）
            .ConfigureAppConfiguration((hosting, config) =>
            {
                // 需要強制存在 appsettings.json 可保留 optional: false
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                // 若有其他自定設定來源可在這邊加
            })
            // Windows 服務（在非 Windows 也不會炸）
            .UseWindowsService(options => options.ServiceName = WindowsServiceName)
            // Serilog：以正式設定覆蓋 bootstrap logger
            .UseSerilog((context, _, loggerConfiguration) =>
                SerilogConfigurator.Configure(loggerConfiguration, context.Configuration, context.HostingEnvironment),
                preserveStaticLogger: true)
            // 服務註冊
            .ConfigureServices((context, services) =>
            {
                var cfg = context.Configuration;

                // ---- Options 綁定（模仿你的 Kaosu 寫法，先取出、Validate、再註冊）----
                var scheduler = cfg.GetSection("Scheduler").Get<SchedulerOptions>() ?? new SchedulerOptions();
                // 需要的話可在 SchedulerOptions 內實作 Validate() 做更嚴格檢查
                // scheduler.Validate();

                var dbOptions = new DatabaseOptions
                {
                    ConnectionString = cfg.GetConnectionString("DefaultConnection") ?? string.Empty
                };
                if (string.IsNullOrWhiteSpace(dbOptions.ConnectionString))
                    throw new InvalidOperationException("請在 appsettings.json 設定 ConnectionStrings:DefaultConnection。");

                var logging = cfg.GetSection("Logging").Get<LoggingOptions>() ?? new LoggingOptions();

                services.AddSingleton(scheduler);
                services.AddSingleton(logging);
                // IOptions 包一層，方便其他組件用 IOptions<DatabaseOptions> 取用
                services.AddSingleton<IOptions<DatabaseOptions>>(_ => Options.Create(dbOptions));

                // ---- 基礎設施與業務服務 ----
                services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
                services.AddSingleton<IDapperRepository, DapperRepository>();
                services.AddSingleton<IJob, ExampleJob>();
                services.AddHostedService<Worker>();
            })
            .Build();
}