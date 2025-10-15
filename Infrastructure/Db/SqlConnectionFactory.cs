using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApiTemplate.Infrastructure.Db;

/// <summary>
/// 提供建立 SQL Server 連線的工廠。
/// </summary>
/// <remarks>Remark: 使用 Dapper 時須透過本工廠取得連線實例。</remarks>
public sealed class SqlConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    /// 初始化 <see cref="SqlConnectionFactory"/> 類別的新執行個體。
    /// </summary>
    /// <param name="configuration">組態來源。</param>
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    /// <summary>
    /// 建立資料庫連線。
    /// </summary>
    /// <returns>可用於 Dapper 操作的資料庫連線。</returns>
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
