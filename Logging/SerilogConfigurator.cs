using System.Text;
using LoaderScheduler.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LoaderScheduler.Logging;

/// <summary>
/// 建立與設定 Serilog 的協助類別。
/// </summary>
/// <remarks>Remark: 提供開發、正式環境以及權限不足時的多種記錄檔路徑判斷。</remarks>
internal static class SerilogConfigurator
{
    private const string DefaultLogDirectoryName = "Logs";
    private const string DefaultLogFilePattern = "app-.log";
    private const int DefaultRetainedLogFileCountLimit = 31;
    private const string DefaultEventLogName = "Application";
    internal const string DefaultEventSourceName = "LoaderScheduler";

    /// <summary>
    /// 建立啟動初期使用的 Serilog 物件。
    /// </summary>
    /// <returns>Serilog <see cref="Logger"/> 實例。</returns>
    public static Logger CreateBootstrapLogger()
    {
        var loggerConfiguration = new LoggerConfiguration();
        Configure(loggerConfiguration, null, null);
        return loggerConfiguration.CreateLogger();
    }

    /// <summary>
    /// 依據設定檔與主機環境設定 Serilog。
    /// </summary>
    /// <param name="loggerConfiguration">Serilog 設定物件。</param>
    /// <param name="configuration">應用程式組態。</param>
    /// <param name="environment">主機環境資訊。</param>
    /// <remarks>Remark: 當組態提供值時將覆寫預設設定。</remarks>
    public static void Configure(LoggerConfiguration loggerConfiguration, IConfiguration? configuration, IHostEnvironment? environment)
    {
        ArgumentNullException.ThrowIfNull(loggerConfiguration);

        var loggingOptions = configuration?.GetSection("Logging").Get<LoggingOptions>();
        var logDirectory = ResolveLogDirectory(loggingOptions, environment);
        var logFilePattern = loggingOptions?.LogFilePattern ?? DefaultLogFilePattern;
        var retainedLogFileCountLimit = loggingOptions?.RetainedFileCountLimit ?? DefaultRetainedLogFileCountLimit;

        loggerConfiguration
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.File(
                path: Path.Combine(logDirectory, logFilePattern),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: retainedLogFileCountLimit,
                shared: true,
                encoding: Encoding.UTF8,
                rollOnFileSizeLimit: true);

        ConfigureEventLogSink(loggerConfiguration, loggingOptions);
    }

    private static string ResolveLogDirectory(LoggingOptions? loggingOptions, IHostEnvironment? environment)
    {
        var directoryName = loggingOptions?.LogDirectoryName ?? DefaultLogDirectoryName;

        try
        {
            if (IsRunningInsideProjectRoot(environment))
            {
                var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
                var devLogs = Path.Combine(projectRoot, directoryName);
                Directory.CreateDirectory(devLogs);
                return devLogs;
            }

            var preferred = Path.Combine(AppContext.BaseDirectory, directoryName);
            Directory.CreateDirectory(preferred);
            return preferred;
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException || ex is IOException)
        {
            var fallback = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                DefaultEventSourceName,
                directoryName);
            Directory.CreateDirectory(fallback);
            Serilog.Debugging.SelfLog.WriteLine("Log dir fallback to {0}: {1}", fallback, ex.Message);
            return fallback;
        }
    }

    private static bool IsRunningInsideProjectRoot(IHostEnvironment? environment)
    {
        if (environment?.IsDevelopment() != true)
        {
            return false;
        }

        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        return Directory.Exists(Path.Combine(projectRoot, "bin"));
    }

    private static void ConfigureEventLogSink(LoggerConfiguration loggerConfiguration, LoggingOptions? loggingOptions)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        try
        {
            var eventSource = loggingOptions?.EventSourceName ?? DefaultEventSourceName;
            var logName = loggingOptions?.EventLogName ?? DefaultEventLogName;

            loggerConfiguration.WriteTo.EventLog(
                source: eventSource,
                logName: logName,
                manageEventSource: true,
                restrictedToMinimumLevel: LogEventLevel.Information);
        }
        catch (Exception exception)
        {
            Serilog.Debugging.SelfLog.WriteLine("Failed to configure EventLog sink: {0}", exception);
        }
    }
}
