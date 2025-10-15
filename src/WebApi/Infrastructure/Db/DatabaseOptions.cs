namespace WebApiTemplate.Infrastructure.Db;

/// <summary>
/// 定義資料庫連線相關設定，方便透過依賴注入統一管理。
/// </summary>
/// <remarks>
/// 使用常數 <see cref="SectionName"/> 對應 appsettings 中的節點，避免硬編碼字串。
/// </remarks>
public sealed class DatabaseOptions
{
    /// <summary>
    /// 組態檔中對應的節點名稱。
    /// </summary>
    public const string SectionName = "Database";

    /// <summary>
    /// SQL Server 連線字串。
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
}
