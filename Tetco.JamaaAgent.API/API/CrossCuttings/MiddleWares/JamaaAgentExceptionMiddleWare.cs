using Application.Common.Exceptions;
using Application.Common.Utilities;
using Domain.Common.Exceptions;
using Domain.Common.Patterns;

namespace API.CrossCuttings.MiddleWares;

public class JamaaAgentExceptionMiddleWare : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {

            var res = ex switch
            {
                BaseJamaaAgentException hubEx => hubEx.ToResultError(),
                _ => ex.ToResultError()
            };


            await WriteExceptionToResponse(context, ex, res);
        }
    }
    private async Task WriteExceptionToResponse(HttpContext httpContext, Exception ex, Result res)
    {
        if (ex is JamaaAgentValidationException _)
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        else if (ex is JamaaAgentForbiddenAccessException _)
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        else if (ex is JamaaAgentInValidOperationException _)
            httpContext.Response.StatusCode = StatusCodes.Status200OK;
        else if (ex is UnauthorizedAccessException _)
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        else
            httpContext.Response.StatusCode = StatusCodes.Status200OK;

        await httpContext.Response.WriteAsJsonAsync(res);
    }
}
