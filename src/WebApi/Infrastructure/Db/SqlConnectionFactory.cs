namespace WebApiTemplate.Infrastructure.Db;

using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

/// <summary>
/// 建立資料庫連線的工廠，集中管理連線設定與生命週期。
/// </summary>
/// <remarks>
/// 之後可於此處加入連線池監控或重試機制，避免重複散落於各服務。
/// </remarks>
public sealed class SqlConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    /// 透過 <see cref="IOptions{TOptions}"/> 取得連線設定，方便環境切換。
    /// </summary>
    /// <param name="options">包含資料庫連線字串的設定。</param>
    public SqlConnectionFactory(IOptions<DatabaseOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _connectionString = options.Value.ConnectionString;
    }

    /// <summary>
    /// 建立新的 <see cref="IDbConnection"/> 實例。
    /// </summary>
    /// <returns>可供 Dapper 使用的資料庫連線。</returns>
    /// <remarks>
    /// 交由呼叫端控制連線開啟與釋放，確保資源管理清楚。
    /// </remarks>
    public IDbConnection CreateConnection()
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        return new SqlConnection(_connectionString);
    }
}
