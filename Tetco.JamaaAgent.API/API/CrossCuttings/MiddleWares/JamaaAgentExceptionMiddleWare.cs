using Application.Common.Exceptions;
using Application.Common.Utilities;
using Domain.Common.Exceptions;
using Domain.Common.Patterns;

namespace API.CrossCuttings.MiddleWares;

public class JamaaAgentExceptionMiddleWare : IMiddleware
{
    private readonly ILogger<JamaaAgentExceptionMiddleWare> _logger;

    public JamaaAgentExceptionMiddleWare(ILogger<JamaaAgentExceptionMiddleWare> logger)
    {
        _logger = logger;
    }
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
            _logger.LogError(ex.Message, res);
            await WriteExceptionToResponse(context, ex, res);
        }
    }
    private async Task WriteExceptionToResponse(HttpContext httpContext, Exception ex, Result res)
    {
        httpContext.Response.StatusCode = ex switch
        {
            JamaaAgentValidationException => StatusCodes.Status400BadRequest,
            JamaaAgentForbiddenAccessException _ => StatusCodes.Status403Forbidden,
            JamaaAgentInValidOperationException _ => StatusCodes.Status200OK,
            UnauthorizedAccessException _ => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status200OK,
        };

        if (httpContext.Response.StatusCode != StatusCodes.Status200OK)
            _logger.LogError(ex.Message, ex);

        await httpContext.Response.WriteAsJsonAsync(res);
    }
}
