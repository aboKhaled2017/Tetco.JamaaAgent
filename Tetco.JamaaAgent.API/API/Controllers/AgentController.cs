using API.CrossCuttings.Authorization;
using Application.NaqelAgent.Queries.Students.GetPage;
using Application.NaqelAgent.Queries.Students.GetStudentMetaData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class AgentController : ApiControllerBase
{


    [HttpPost("getPage")]
    public async Task<IActionResult> GetPageOfStudents(GetPageOfStudentQuery request)
    {
        return Ok(await Mediator.Send(request));
    }


    [HttpPost("getMetaData")]
    public async Task<IActionResult> GetMetaData(GetAgentMetaDataQuery request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPost("getDynamicQueryData")]
    public async Task<IActionResult> GetDynamicQueryData(GetDynamicQueryDataReq request)
    {
        return Ok(await Mediator.Send(request));
    }


    [HttpGet("healthCheck")]
    public async Task<IActionResult> AgentHealthCheck()
    {

        return Ok("works fine");
    }
}
