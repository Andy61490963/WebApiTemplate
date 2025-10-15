namespace WebApiTemplate.Application.DTOs.Auth;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// 用於註銷指定更新權杖的請求。
/// </summary>
/// <remarks>
/// 透過模型驗證避免空白權杖請求造成資源浪費。
/// </remarks>
public sealed class LogoutRequest
{
    /// <summary>
    /// 準備撤銷的更新權杖值。
    /// </summary>
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}
