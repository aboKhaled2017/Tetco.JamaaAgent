using API.CrossCuttings.Authorization;
using Application.Agent.Queries.GetAgentVersion;
using Application.Agent.Queries.GetDynamicQueryData;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class AgentController : ApiControllerBase
{

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

    [HttpGet("getAgentVersion")]
    public async Task<IActionResult> GetAgentVersion(GetAgentVersionReq request)
    {
        return Ok(await Mediator.Send(request));
    }
}
