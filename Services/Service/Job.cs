using LoaderScheduler.Services.Interface;
using Microsoft.Extensions.Logging;

namespace LoaderScheduler.Services.Service;

/// <summary>
/// 提供作業的共同行為。
/// </summary>
/// <remarks>Remark: 集中例外處理與事件紀錄邏輯。</remarks>
public abstract class Job : IJob
{
    private readonly ILogger _logger;

    /// <summary>
    /// 初始化 <see cref="Job"/> 類別的新執行個體。
    /// </summary>
    /// <param name="logger">紀錄器。</param>
    protected Job(ILogger logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Job {JobName} 開始。", GetType().Name);
            var domainEvents = await ExecuteCoreAsync(cancellationToken).ConfigureAwait(false);
            LogDomainEvents(domainEvents);
            _logger.LogInformation("Job {JobName} 結束。", GetType().Name);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Job {JobName} 已被取消。", GetType().Name);
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Job {JobName} 執行失敗。", GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// 由子類別實作的實際作業流程。
    /// </summary>
    /// <param name="cancellationToken">取消權杖。</param>
    /// <returns>作業產生的事件集合。</returns>
    protected abstract Task<IReadOnlyCollection<IDomainEvent>> ExecuteCoreAsync(CancellationToken cancellationToken);

    private void LogDomainEvents(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var description = (domainEvent as DomainEvent)?.Describe() ?? domainEvent.Name;
            _logger.LogInformation(
                "偵測到事件 {EventName} 於 {OccurredOn:o}，內容：{Description}",
                domainEvent.Name,
                domainEvent.OccurredOn,
                description);
        }
    }
}
