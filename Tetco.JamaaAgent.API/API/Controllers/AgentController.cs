using API.CrossCuttings.Authorization;
using Application.Common.Exceptions;
using Application.NaqelAgent.Queries.GetAllNewtemplates;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class AgentController : ApiControllerBase
{
    //private ISender _mediator;
    //public AgentController()
    //{
    //    _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    //}
    

    //[HttpGet("getData")]
    //public async Task<IActionResult> GetAgentData()
    //{

    //    return Ok();
    //}

    [HttpPost("GetAllStudents")]
    public async Task<IActionResult> GetAllStudents(GetAllStudentssQuery students)
    {
        return Ok(await Mediator.Send(students));
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
