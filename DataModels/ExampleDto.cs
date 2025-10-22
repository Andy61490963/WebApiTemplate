namespace LoaderScheduler.DataModels;

/// <summary>
/// 範例資料傳輸物件。
/// </summary>
/// <remarks>Remark: 此類別對應資料庫中的示範資料表欄位。</remarks>
public sealed class ExampleDto
{
    /// <summary>
    /// 取得或設定範例識別碼。
    /// </summary>
    public int ExampleId { get; set; }

    /// <summary>
    /// 取得或設定範例名稱。
    /// </summary>
    public string? ExampleName { get; set; }

    /// <summary>
    /// 取得或設定資料最後更新時間。
    /// </summary>
    public DateTime LastUpdatedOn { get; set; }
}
