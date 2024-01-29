using Application.Common.Models;
using Application.Common.Utilities;
using Domain.Common.Patterns;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Application.AgentLogs.Queries.GetAgentLogs
{
    public sealed record GetAgentLogsRes(IEnumerable<LogEntry> Logs);
    public sealed class GetAgentLogsReq : IRequest<Result<GetAgentLogsRes>>
    {
       public DateOnly Date { get; set; }
    }
    public sealed class GetAgentLogsHandler : IRequestHandler<GetAgentLogsReq, Result<GetAgentLogsRes>>
    {
        ILogger<GetAgentLogsHandler> _logger;
        private readonly Helper _helper;

        public GetAgentLogsHandler(ILogger<GetAgentLogsHandler> logger, Helper helper)
        {
            _logger = logger;
            _helper = helper;
        }

        public async Task<Result<GetAgentLogsRes>> Handle(GetAgentLogsReq request, CancellationToken cancellationToken)
        {
            Task.CompletedTask.Wait();
            try
            {
                return _helper.GetLogsByDate(request.Date,_logger);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving agent logs");
                return Result<GetAgentLogsRes>.Failure("500", "Internal Server Error");
            }
        }

        

    }

}

