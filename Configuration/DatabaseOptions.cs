namespace LoaderScheduler.Configuration;

/// <summary>
/// 代表資料庫連線相關設定。
/// </summary>
/// <remarks>Remark: ConnectionString 需為有效的 SQL Server 連線字串。</remarks>
public sealed class DatabaseOptions
{
    /// <summary>
    /// 取得或設定預設資料庫連線字串。
    /// </summary>
    public required string ConnectionString { get; set; }
}
