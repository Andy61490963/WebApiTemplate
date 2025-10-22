namespace LoaderScheduler.Services.Interface;

/// <summary>
/// 定義排程作業介面。
/// </summary>
/// <remarks>Remark: 所有作業需實作此介面以利工作排程器呼叫。</remarks>
public interface IJob
{
    /// <summary>
    /// 執行作業主要流程。
    /// </summary>
    /// <param name="cancellationToken">取消權杖。</param>
    Task ExecuteAsync(CancellationToken cancellationToken);
}
