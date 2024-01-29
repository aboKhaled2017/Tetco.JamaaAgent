﻿using API.CrossCuttings.Authorization;
using Application.AgentLogs.Command.DeleteAgentLogFile;
using Application.AgentLogs.Queries.GetAgentLogs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class AgentLogsController : ApiControllerBase
{


    [HttpGet("{date}")]
    public async Task<IActionResult> GetLogs(DateOnly date)
    {
        if (date == DateOnly.MinValue)
            return BadRequest("Invalid date format.");

        var logs = await Mediator.Send(new GetAgentLogsReq { Date = date });
        return Ok(logs);
    }

    [HttpDelete("{date}")]
    public async Task<IActionResult> DeleteLogs(DateOnly date)
    {
        var result= await Mediator.Send(new DeleteAgentLogFileReq { Date = date });
        return Ok(result);
    }

}
