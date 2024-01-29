using API.CrossCuttings.Authorization;
using Application.Agent.Queries.Students.GetPage;
using Application.Agent.Queries.Students.GetStudentMetaData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class StudentController : ApiControllerBase
{
    [HttpPost("getMetaData")]
    public async Task<IActionResult> GetMetaData(GetAgentMetaDataQuery request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPost("getPage")]
    public async Task<IActionResult> GetPageOfStudents(GetPageOfStudentQuery request)
    {
        return Ok(await Mediator.Send(request));
    }

}
