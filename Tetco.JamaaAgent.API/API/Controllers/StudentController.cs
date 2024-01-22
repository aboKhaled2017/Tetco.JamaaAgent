using API.CrossCuttings.Authorization;
using Application.NaqelAgent.Queries.Students.GetPage;
using Application.NaqelAgent.Queries.Students.GetStudentMetaData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class StudentController : ApiControllerBase
{


    [HttpPost("getPage")]
    public async Task<IActionResult> GetPageOfStudents(GetPageOfStudentQuery students)
    {
        return Ok(await Mediator.Send(students));
    }


    [HttpPost("getMetaData")]
    public async Task<IActionResult> getMetaData(GetStudentsMetaDataQuery students)
    {
        return Ok(await Mediator.Send(students));
    }


    [HttpGet("healthCheck")]
    public async Task<IActionResult> AgentHealthCheck()
    {

        return Ok("works fine");
    }
}
