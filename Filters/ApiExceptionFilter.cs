using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiTemplate.Filters;

/// <summary>
/// 處理全域例外並統一回應格式的篩選器。
/// </summary>
/// <remarks>Remark: 目前提供最小化的錯誤轉換邏輯。</remarks>
public sealed class ApiExceptionFilter : IExceptionFilter
{
    /// <summary>
    /// 當 Action 發生例外時觸發。
    /// </summary>
    /// <param name="context">篩選器內容。</param>
    public void OnException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "系統錯誤",
            Detail = context.Exception.Message,
            Status = StatusCodes.Status500InternalServerError
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
