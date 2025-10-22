namespace LoaderScheduler.Configuration;

/// <summary>
/// 代表 Serilog 相關設定。
/// </summary>
/// <remarks>Remark: 若未提供值則會使用 SerilogConfigurator 的預設值。</remarks>
public sealed class LoggingOptions
{
    /// <summary>
    /// 取得或設定記錄檔儲存目錄名稱。
    /// </summary>
    public string? LogDirectoryName { get; set; }

    /// <summary>
    /// 取得或設定記錄檔名稱樣板。
    /// </summary>
    public string? LogFilePattern { get; set; }

    /// <summary>
    /// 取得或設定保留的記錄檔數量上限。
    /// </summary>
    public int? RetainedFileCountLimit { get; set; }

    /// <summary>
    /// 取得或設定 Windows Event Log 名稱。
    /// </summary>
    public string? EventLogName { get; set; }

    /// <summary>
    /// 取得或設定 Windows Event Source 名稱。
    /// </summary>
    public string? EventSourceName { get; set; }
}
