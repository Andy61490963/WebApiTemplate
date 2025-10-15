namespace WebApiTemplate.Filters;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// 統一攔截未處理的例外並轉換為一致的 API 回應格式。
/// </summary>
/// <remarks>
/// 減少控制器重複的 try-catch，維持橫切關注點的一致性。
/// </remarks>
public sealed class ApiExceptionFilter : IExceptionFilter
{
    /// <summary>
    /// 例外發生時觸發，組織成 JSON 錯誤回應。
    /// </summary>
    /// <param name="context">攜帶例外與 HttpContext 的資訊。</param>
    /// <remarks>
    /// 後續可擴充記錄日誌或整合 APM。
    /// </remarks>
    public void OnException(ExceptionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = context.Exception.Message,
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
}
