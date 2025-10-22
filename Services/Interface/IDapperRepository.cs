using System.Data;

namespace LoaderScheduler.Services.Interface;

/// <summary>
/// 定義使用 Dapper 進行資料庫操作的方法。
/// </summary>
/// <remarks>Remark: 提供查詢與命令執行的抽象，以利單元測試。</remarks>
public interface IDapperRepository
{
    /// <summary>
    /// 執行查詢並回傳資料列集合。
    /// </summary>
    /// <typeparam name="T">資料模型型別。</typeparam>
    /// <param name="sql">查詢語法。</param>
    /// <param name="parameters">查詢參數。</param>
    /// <param name="transaction">資料庫交易。</param>
    /// <param name="commandTimeout">命令逾時秒數。</param>
    /// <returns>查詢結果集合。</returns>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters, IDbTransaction? transaction, int? commandTimeout, CancellationToken cancellationToken);

    /// <summary>
    /// 執行命令並回傳受影響列數。
    /// </summary>
    /// <param name="sql">命令語法。</param>
    /// <param name="parameters">命令參數。</param>
    /// <param name="transaction">資料庫交易。</param>
    /// <param name="commandTimeout">命令逾時秒數。</param>
    /// <returns>受影響的列數。</returns>
    Task<int> ExecuteAsync(string sql, object? parameters, IDbTransaction? transaction, int? commandTimeout, CancellationToken cancellationToken);
}
