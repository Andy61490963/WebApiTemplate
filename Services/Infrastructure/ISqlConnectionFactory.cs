using System.Data.Common;

namespace LoaderScheduler.Services.Infrastructure;

/// <summary>
/// 定義資料庫連線工廠介面。
/// </summary>
/// <remarks>Remark: 抽象化連線產生，以利測試與資源管理。</remarks>
public interface ISqlConnectionFactory
{
    /// <summary>
    /// 建立開啟中的資料庫連線。
    /// </summary>
    /// <param name="cancellationToken">取消權杖。</param>
    /// <returns>可用的 <see cref="DbConnection"/>。</returns>
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken);
}
