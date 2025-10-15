namespace WebApiTemplate.Application.DTOs.Auth;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// 封裝登入流程所需的輸入資料。
/// </summary>
/// <remarks>
/// 透過資料註解確保 API 層面即可驗證輸入，避免未經檢查的資料進入基礎設施層。
/// </remarks>
public sealed class LoginRequest
{
    /// <summary>
    /// 使用者登入帳號。
    /// </summary>
    /// <remarks>保留長度限制，可視資料庫欄位長度調整，避免暴力攻擊造成效能問題。</remarks>
    [Required]
    [StringLength(256, MinimumLength = 3)]
    public string UserName { get; init; } = string.Empty;

    /// <summary>
    /// 使用者登入密碼。
    /// </summary>
    /// <remarks>未於此階段保存實際密碼，只作為服務驗證的輸入。</remarks>
    [Required]
    [StringLength(256, MinimumLength = 6)]
    public string Password { get; init; } = string.Empty;
}
