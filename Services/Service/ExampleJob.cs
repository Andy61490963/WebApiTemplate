using LoaderScheduler.DataModels;
using LoaderScheduler.Services.Interface;
using Microsoft.Extensions.Logging;

namespace LoaderScheduler.Services.Service;

/// <summary>
/// 範例排程作業：定期查詢資料並輸出結果摘要。
/// </summary>
/// <remarks>Remark: 示範如何整合 Dapper 與事件機制。</remarks>
public sealed class ExampleJob : Job
{
    private const string ExampleQuery = @"/**/
SELECT TOP (5)
       ExampleId,
       ExampleName,
       LastUpdatedOn
FROM   dbo.Example
ORDER BY LastUpdatedOn DESC;";

    private readonly IDapperRepository _repository;

    /// <summary>
    /// 初始化 <see cref="ExampleJob"/> 類別的新執行個體。
    /// </summary>
    /// <param name="repository">Dapper 資料存取元件。</param>
    /// <param name="logger">紀錄器。</param>
    public ExampleJob(IDapperRepository repository, ILogger<ExampleJob> logger)
        : base(logger)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    protected override async Task<IReadOnlyCollection<IDomainEvent>> ExecuteCoreAsync(CancellationToken cancellationToken)
    {
        var parameters = CreateParameters();
        var records = await _repository.QueryAsync<ExampleDto>(ExampleQuery, parameters, transaction: null, commandTimeout: null, cancellationToken).ConfigureAwait(false);
        return new[] { CreateSummaryEvent(records) };
    }

    private static object CreateParameters()
    {
        return new { };
    }

    private static IDomainEvent CreateSummaryEvent(IEnumerable<ExampleDto> records)
    {
        var list = records.ToList();
        var latest = list.FirstOrDefault();
        var summary = latest is null
            ? "未取得任何資料。"
            : $"最新資料：Id={latest.ExampleId}, Name={latest.ExampleName}, UpdatedOn={latest.LastUpdatedOn:O}";
        return new ExampleDataLoadedEvent(summary, list.Count);
    }

    private sealed class ExampleDataLoadedEvent : DomainEvent
    {
        public ExampleDataLoadedEvent(string summary, int count)
            : base(nameof(ExampleDataLoadedEvent))
        {
            Summary = summary;
            Count = count;
        }

        public string Summary { get; }

        public int Count { get; }

        public override string Describe() => $"{Summary} (筆數：{Count})";
    }
}
