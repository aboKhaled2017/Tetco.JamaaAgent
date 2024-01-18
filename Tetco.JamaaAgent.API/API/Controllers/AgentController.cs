using API.CrossCuttings.Authorization;
using Application.Common.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class AgentController : ControllerBase
{
    private ISender _mediator;
    public AgentController()
    {
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }

    private ProblemDetails CreateValidationProblem(IEnumerable<ValidationFailure> errors)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "Validation error",
            Status = StatusCodes.Status400BadRequest,
            Detail = "One or more validation errors occurred.",
            Instance = HttpContext.Request.Path
        };

        foreach (var error in errors.GroupBy(x=>x.PropertyName))
        {
            problemDetails.Extensions.Add(error.Key, error.Select(x=>x.ErrorMessage));
        }

        return problemDetails;
    }
    protected IActionResult NotValidRequest(ValidationResult validationResult)
    {
        throw new JamaaAgentValidationException(validationResult.Errors);
    }

    [HttpGet("getData")]
    public async Task<IActionResult> GetAgentData()
    {

        return Ok();
    }

    [HttpGet("getMetaData")]
    public async Task<IActionResult> GetAgentMetaData()
    {

        return Ok();
    }


    [HttpGet("healthCheck")]
    public async Task<IActionResult> AgentHealthCheck()
    {

        return Ok("works fine");
    }
}
