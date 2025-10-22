using System.Data.Common;
using System.Data.SqlClient;
using LoaderScheduler.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LoaderScheduler.Services.Infrastructure;

/// <summary>
/// 使用設定檔建立 SQL Server 連線的工廠。
/// </summary>
/// <remarks>Remark: 透過 <see cref="IOptions{TOptions}"/> 取得連線字串並確保例外處理記錄。</remarks>
public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly DatabaseOptions _options;
    private readonly ILogger<SqlConnectionFactory> _logger;

    /// <summary>
    /// 初始化 <see cref="SqlConnectionFactory"/> 類別的新執行個體。
    /// </summary>
    /// <param name="options">資料庫設定。</param>
    /// <param name="logger">紀錄器。</param>
    public SqlConnectionFactory(IOptions<DatabaseOptions> options, ILogger<SqlConnectionFactory> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        SqlConnection connection;
        try
        {
            connection = new SqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "開啟資料庫連線失敗。");
            throw;
        }

        return connection;
    }
}
