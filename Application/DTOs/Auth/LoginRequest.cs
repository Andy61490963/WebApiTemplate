using System.ComponentModel.DataAnnotations;

namespace WebApiTemplate.Application.DTOs.Auth;

/// <summary>
/// 使用者登入請求的資料模型。
/// </summary>
/// <remarks>Remark: 負責接收客戶端登入所需的基本認證資訊。</remarks>
public sealed class LoginRequest
{
    /// <summary>
    /// 帳號識別字串。
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// 使用者登入密碼。
    /// </summary>
    [Required]
    [StringLength(256, MinimumLength = 8)]
    public string Password { get; init; } = string.Empty;
}
