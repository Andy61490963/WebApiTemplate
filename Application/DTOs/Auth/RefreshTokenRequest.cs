using System.ComponentModel.DataAnnotations;

namespace WebApiTemplate.Application.DTOs.Auth;

/// <summary>
/// 刷新令牌請求的資料模型。
/// </summary>
/// <remarks>Remark: 由客戶端於存取令牌過期時呼叫。</remarks>
public sealed class RefreshTokenRequest
{
    /// <summary>
    /// 原本的刷新令牌字串。
    /// </summary>
    [Required]
    [StringLength(512, MinimumLength = 16)]
    public string RefreshToken { get; init; } = string.Empty;
}
