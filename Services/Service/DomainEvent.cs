using LoaderScheduler.Services.Interface;

namespace LoaderScheduler.Services.Service;

/// <summary>
/// 領域事件的基底實作。
/// </summary>
/// <remarks>Remark: 提供統一的事件資料結構與說明。</remarks>
public abstract class DomainEvent : IDomainEvent
{
    /// <summary>
    /// 初始化 <see cref="DomainEvent"/> 類別的新執行個體。
    /// </summary>
    /// <param name="name">事件名稱。</param>
    protected DomainEvent(string name)
    {
        Name = name;
        OccurredOn = DateTime.UtcNow;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public DateTime OccurredOn { get; }

    /// <summary>
    /// 取得事件描述文字。
    /// </summary>
    /// <returns>事件描述。</returns>
    /// <remarks>Remark: 預設回傳事件名稱，可由子類別覆寫。</remarks>
    public virtual string Describe() => Name;
}
