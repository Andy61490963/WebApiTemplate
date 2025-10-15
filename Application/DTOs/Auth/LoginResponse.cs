using System;

namespace WebApiTemplate.Application.DTOs.Auth;

/// <summary>
/// 使用者登入成功後的回應資料模型。
/// </summary>
/// <remarks>Remark: 回傳給客戶端的 JWT 與刷新令牌資訊。</remarks>
public sealed class LoginResponse
{
    /// <summary>
    /// JWT 存取令牌字串。
    /// </summary>
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>
    /// 刷新令牌字串。
    /// </summary>
    public string RefreshToken { get; init; } = string.Empty;

    /// <summary>
    /// 存取令牌的到期時間（UTC）。
    /// </summary>
    public DateTime ExpiresAtUtc { get; init; }
}
