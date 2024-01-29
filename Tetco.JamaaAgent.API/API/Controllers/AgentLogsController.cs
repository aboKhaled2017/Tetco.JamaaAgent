using API.CrossCuttings.Authorization;
using Application.AgentLogs.Queries.GetAgentLogs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class AgentLogsController : ApiControllerBase
{

    [HttpPost("getAgentLogsByDate")]
    public async Task<IActionResult> GetAgentLogsByDate(GetAgentLogsReq request)
    {
        return Ok(await Mediator.Send(request));
    }

}
