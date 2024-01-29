using Application.Common.Models;
using Application.Common.Utilities;
using Domain.Common.Patterns;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        private readonly LogReader _logReader;

        public GetAgentLogsHandler(ILogger<GetAgentLogsHandler> logger, LogReader logReader)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logReader = logReader ?? throw new ArgumentNullException(nameof(logReader));
        }

        public async Task<Result<GetAgentLogsRes>> Handle(GetAgentLogsReq request, CancellationToken cancellationToken)
        {
            try
            {
                var logEntries = _logReader.ReadLogs(request.Date);
                return Result<GetAgentLogsRes>.Success("Data retrieved successfully").WithData(new GetAgentLogsRes(logEntries.ToList()));
            }
            
            catch (FileNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return Result<GetAgentLogsRes>.Failure("404", ex.Message).WithData(new GetAgentLogsRes(new List<LogEntry>()));
            }
            catch (IOException ex)
            {
                _logger.LogInformation(ex.Message);
                return Result<GetAgentLogsRes>.Failure("500", ex.Message).WithData(new GetAgentLogsRes(new List<LogEntry>()));
            }
            catch (JsonException ex)
            {
                var errorDes = $"Error deserializing JSON. Error: {ex.Message}";
                _logger.LogError(errorDes);
                return Result<GetAgentLogsRes>.Failure("500", errorDes).WithData(new GetAgentLogsRes(new List<LogEntry>()));
            }
            catch (Exception ex)
            {
                var errorDes = $"Unexpected error: {ex.Message}";
                _logger.LogError(errorDes);
                return Result<GetAgentLogsRes>.Failure("500", errorDes).WithData(new GetAgentLogsRes(new List<LogEntry>()));
            }
        }

        

    }

}

