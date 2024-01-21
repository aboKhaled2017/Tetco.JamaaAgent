using Application.Common.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        //private ProblemDetails CreateValidationProblem(IEnumerable<ValidationFailure> errors)
        //{
        //    var problemDetails = new ProblemDetails
        //    {
        //        Title = "Validation error",
        //        Status = StatusCodes.Status400BadRequest,
        //        Detail = "One or more validation errors occurred.",
        //        Instance = HttpContext.Request.Path
        //    };

        //    foreach (var error in errors.GroupBy(x=>x.PropertyName))
        //    {
        //        problemDetails.Extensions.Add(error.Key, error.Select(x=>x.ErrorMessage));
        //    }

        //    return problemDetails;
        //}
        private ProblemDetails CreateValidationProblem(IEnumerable<ValidationFailure> errors)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Instance = HttpContext.Request.Path
            };

            foreach (var error in errors.GroupBy(x => x.PropertyName))
            {
                problemDetails.Extensions.Add(error.Key, error.Select(x => x.ErrorMessage));
            }

            return problemDetails;
        }
        protected IActionResult NotValidRequest(ValidationResult validationResult)
        {
            return new ObjectResult(CreateValidationProblem(validationResult.Errors))
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
        //protected IActionResult NotValidRequest(ValidationResult validationResult)
        //{
        //    throw new JamaaAgentValidationException(validationResult.Errors);
        //}

    }
}
