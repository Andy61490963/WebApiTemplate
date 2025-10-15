using WebApiTemplate.Application.DTOs.Auth;

namespace WebApiTemplate.Application.Interfaces;

/// <summary>
/// 定義認證服務的操作介面。
/// </summary>
/// <remarks>Remark: 提供登入、刷新令牌與登出等核心流程。</remarks>
public interface IAuthService
{
    /// <summary>
    /// 處理登入流程並產生 JWT。
    /// </summary>
    /// <param name="request">登入請求內容。</param>
    /// <param name="cancellationToken">取消作業權杖。</param>
    /// <returns>登入成功後的回應資料。</returns>
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// 刷新使用者的 JWT。
    /// </summary>
    /// <param name="request">刷新令牌請求內容。</param>
    /// <param name="cancellationToken">取消作業權杖。</param>
    /// <returns>新的登入回應資料。</returns>
    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// 失效指定的刷新令牌。
    /// </summary>
    /// <param name="request">登出請求內容。</param>
    /// <param name="cancellationToken">取消作業權杖。</param>
    /// <returns>非同步作業。</returns>
    Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken);
}
