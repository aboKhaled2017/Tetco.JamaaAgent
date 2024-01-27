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
    public async Task<IActionResult> GetPageOfStudents(GetPageOfStudentQuery students)
    {
        return Ok(await Mediator.Send(students));
    }


    [HttpPost("getMetaData")]
    public async Task<IActionResult> GetMetaData(GetStudentsMetaDataQuery students)
    {
        return Ok(await Mediator.Send(students));
    }

    [HttpPost("getDynamicQueryData")]
    public async Task<IActionResult> GetDynamicQueryData(GetDynamicQueryDataReq students)
    {
        return Ok(await Mediator.Send(students));
    }


    [HttpGet("healthCheck")]
    public async Task<IActionResult> AgentHealthCheck()
    {

        return Ok("works fine");
    }
}
