namespace WebApiTemplate.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Application.DTOs.Auth;
using WebApiTemplate.Application.Interfaces;

/// <summary>
/// 提供登入、更新權杖與登出的 API 端點骨架。
/// </summary>
/// <remarks>
/// 依賴 <see cref="IAuthService"/> 以保持控制器的簡潔與可測試性。
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public sealed class LoginController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// 建構式注入驗證服務，維持控制器的單一責任。
    /// </summary>
    /// <param name="authService">處理驗證流程的服務。</param>
    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 進行使用者登入。
    /// </summary>
    /// <param name="request">登入所需的帳號密碼資訊。</param>
    /// <param name="cancellationToken">允許客戶端取消請求。</param>
    /// <returns>登入結果與權杖資訊。</returns>
    /// <remarks>
    /// 使用 <see cref="ApiControllerAttribute"/> 的模型驗證，可在進入服務前阻擋非法輸入。
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// 透過更新權杖取得新的存取權杖。
    /// </summary>
    /// <param name="request">包含舊權杖的請求。</param>
    /// <param name="cancellationToken">允許客戶端取消請求。</param>
    /// <returns>新的權杖資訊。</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshTokenAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// 撤銷指定更新權杖，以完成登出流程。
    /// </summary>
    /// <param name="request">包含欲撤銷權杖的請求。</param>
    /// <param name="cancellationToken">允許客戶端取消請求。</param>
    /// <returns>無內容的成功回應。</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(request, cancellationToken).ConfigureAwait(false);
        return NoContent();
    }
}
