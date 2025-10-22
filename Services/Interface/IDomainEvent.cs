namespace LoaderScheduler.Services.Interface;

/// <summary>
/// 定義領域事件的基本介面。
/// </summary>
/// <remarks>Remark: 用於跨服務傳遞處理結果或狀態。</remarks>
public interface IDomainEvent
{
    /// <summary>
    /// 取得事件名稱。
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 取得事件發生時間。
    /// </summary>
    DateTime OccurredOn { get; }
}
