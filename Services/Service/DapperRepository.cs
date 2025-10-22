using System.Data;
using System.Data.Common;
using Dapper;
using LoaderScheduler.Services.Infrastructure;
using LoaderScheduler.Services.Interface;
using Microsoft.Extensions.Logging;

namespace LoaderScheduler.Services.Service;

/// <summary>
/// 使用 Dapper 實作資料庫存取的共用邏輯。
/// </summary>
/// <remarks>Remark: 集中連線/交易管理並提供非同步 API。</remarks>
public sealed class DapperRepository : IDapperRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<DapperRepository> _logger;

    /// <summary>
    /// 初始化 <see cref="DapperRepository"/> 類別的新執行個體。
    /// </summary>
    /// <param name="connectionFactory">連線工廠。</param>
    /// <param name="logger">紀錄器。</param>
    public DapperRepository(ISqlConnectionFactory connectionFactory, ILogger<DapperRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters, System.Data.IDbTransaction? transaction, int? commandTimeout, CancellationToken cancellationToken)
    {
        return await ExecuteWithConnectionAsync(async connection =>
            await connection.QueryAsync<T>(new CommandDefinition(sql, parameters, transaction, commandTimeout, cancellationToken: cancellationToken)).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> ExecuteAsync(string sql, object? parameters, System.Data.IDbTransaction? transaction, int? commandTimeout, CancellationToken cancellationToken)
    {
        return await ExecuteWithConnectionAsync(async connection =>
            await connection.ExecuteAsync(new CommandDefinition(sql, parameters, transaction, commandTimeout, cancellationToken: cancellationToken)).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> ExecuteWithConnectionAsync<T>(Func<DbConnection, Task<T>> action, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(action);

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            return await action(connection).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Dapper 執行時發生錯誤。");
            throw;
        }
    }
}
