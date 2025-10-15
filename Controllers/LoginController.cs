using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Application.DTOs.Auth;
using WebApiTemplate.Application.Interfaces;

namespace WebApiTemplate.Controllers;

/// <summary>
/// 處理登入相關 API 的控制器。
/// </summary>
/// <remarks>Remark: 負責驅動認證流程的進入點。</remarks>
[ApiController]
[Route("api/[controller]")]
public sealed class LoginController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// 初始化 <see cref="LoginController"/> 類別的新執行個體。
    /// </summary>
    /// <param name="authService">認證服務。</param>
    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 登入系統並取得 JWT。
    /// </summary>
    /// <param name="request">登入請求內容。</param>
    /// <returns>登入結果。</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request, HttpContext.RequestAborted).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// 刷新存取令牌。
    /// </summary>
    /// <param name="request">刷新令牌請求內容。</param>
    /// <returns>新的登入回應。</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request, HttpContext.RequestAborted).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// 登出系統並失效刷新令牌。
    /// </summary>
    /// <param name="request">登出請求內容。</param>
    /// <returns>無內容回應。</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest request)
    {
        await _authService.LogoutAsync(request, HttpContext.RequestAborted).ConfigureAwait(false);
        return NoContent();
    }
}
