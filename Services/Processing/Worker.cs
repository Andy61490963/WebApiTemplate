using LoaderScheduler.Configuration;
using LoaderScheduler.Services.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LoaderScheduler.Services.Processing;

/// <summary>
/// 背景作業主機，負責依據排程設定呼叫指定工作。
/// </summary>
/// <remarks>Remark: 使用 <see cref="PeriodicTimer"/> 以降低 CPU 使用率並確保可取消。</remarks>
public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IJob _job;
    private readonly TimeSpan _interval;

    /// <summary>
    /// 初始化 <see cref="Worker"/> 類別的新執行個體。
    /// </summary>
    /// <param name="logger">紀錄器。</param>
    /// <param name="job">實際執行的工作。</param>
    /// <param name="options">排程設定。</param>
    public Worker(ILogger<Worker> logger, IJob job, IOptions<SchedulerOptions> options)
    {
        _logger = logger;
        _job = job;
        _interval = TimeSpan.FromSeconds(ValidateInterval(options.Value.IntervalSeconds));
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker 啟動，間隔 {IntervalSeconds} 秒。", _interval.TotalSeconds);

        await ExecuteJobSafeAsync(stoppingToken).ConfigureAwait(false);

        using var timer = new PeriodicTimer(_interval);
        while (await WaitForNextTickAsync(timer, stoppingToken).ConfigureAwait(false))
        {
            await ExecuteJobSafeAsync(stoppingToken).ConfigureAwait(false);
        }
    }

    private static int ValidateInterval(int intervalSeconds)
    {
        if (intervalSeconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalSeconds), intervalSeconds, "IntervalSeconds 必須大於零。");
        }

        return intervalSeconds;
    }

    private static async Task<bool> WaitForNextTickAsync(PeriodicTimer timer, CancellationToken cancellationToken)
    {
        try
        {
            return await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    private async Task ExecuteJobSafeAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _job.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Job 執行被取消。");
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Job 執行時發生未處理例外。");
        }
    }
}
