using API.CrossCuttings.Authorization;
using Application.AgentLogs.Command.DeleteAgentLogsFile;
using Application.AgentLogs.Command.DeleteAgentLogsFiles;
using Application.AgentLogs.Queries.GetAgentLogsFileFile;
using Application.AgentLogs.Queries.GetAgentLogsFiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[MustHaveAgentApiKey]
[ApiController]
[Route("api/[controller]")]
public sealed class AgentLogsController : ApiControllerBase
{


    [HttpGet("{date}")]
    public async Task<IActionResult> GetLogsFile(DateOnly date)
    {
        if (date == DateOnly.MinValue)
            return BadRequest("Invalid date format.");

        var logs = await Mediator.Send(new GetAgentLogsFileReq { Date = date });
        return Ok(logs);
    }

    [HttpDelete("{date}")]
    public async Task<IActionResult> DeleteLogsFile(DateOnly date)
    {
        if (date == DateOnly.MinValue)
            return BadRequest("Invalid date format.");

        var result= await Mediator.Send(new DeleteAgentLogsFileReq { Date = date });
        return Ok(result);
    }

    [HttpGet("getLogsFiles")]
    public async Task<IActionResult> GetLogsFiles(GetAgentLogsFilesReq req)
    {
        if (req.StartDate == DateOnly.MinValue || req.EndDate == DateOnly.MinValue)
        {
            ModelState.AddModelError("DateValidation", "Invalid date format. Both start and end dates are required.");
            return BadRequest(ModelState);
        }
        else if (req.StartDate > req.EndDate)
        {
            ModelState.AddModelError("DateValidation", "Error: Start date must be before or equal to end date.");
            return BadRequest(ModelState);
        }

        var logs = await Mediator.Send(req);
        return Ok(logs);
    }

    [HttpDelete("deleteLogsFiles")]
    public async Task<IActionResult> DeleteLogsFiles(DeleteAgentLogsFilesReq req)
    {
        if (req.StartDate == DateOnly.MinValue || req.EndDate == DateOnly.MinValue)
        {
            ModelState.AddModelError("DateValidation", "Invalid date format. Both start and end dates are required.");
            return BadRequest(ModelState);
        }
        else if (req.StartDate > req.EndDate)
        {
            ModelState.AddModelError("DateValidation", "Error: Start date must be before or equal to end date.");
            return BadRequest(ModelState);
        }

        var result = await Mediator.Send(req);
        return Ok(result);
    }


}
