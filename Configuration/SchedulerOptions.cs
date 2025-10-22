using System.ComponentModel.DataAnnotations;

namespace LoaderScheduler.Configuration;

/// <summary>
/// 代表排程執行的設定。
/// </summary>
/// <remarks>Remark: IntervalSeconds 須大於零以避免密集輪詢造成資源耗盡。</remarks>
public sealed class SchedulerOptions
{
    /// <summary>
    /// 取得或設定排程間隔（秒）。
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "IntervalSeconds 必須大於零。")]
    public int IntervalSeconds { get; set; }
}
