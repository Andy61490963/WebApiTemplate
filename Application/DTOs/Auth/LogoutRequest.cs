using System.ComponentModel.DataAnnotations;

namespace WebApiTemplate.Application.DTOs.Auth;

/// <summary>
/// 使用者登出請求的資料模型。
/// </summary>
/// <remarks>Remark: 用於使伺服端失效指定刷新令牌。</remarks>
public sealed class LogoutRequest
{
    /// <summary>
    /// 準備失效的刷新令牌字串。
    /// </summary>
    [Required]
    [StringLength(512, MinimumLength = 16)]
    public string RefreshToken { get; init; } = string.Empty;
}
