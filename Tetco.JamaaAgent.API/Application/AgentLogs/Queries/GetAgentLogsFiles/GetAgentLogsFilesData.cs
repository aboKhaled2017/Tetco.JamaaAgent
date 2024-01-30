using Application.Common.Models;
using Application.Common.Utilities;
using Domain.Common.Patterns;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.AgentLogs.Queries.GetAgentLogsFiles
{
    public sealed record GetAgentLogsFilesRes(IEnumerable<LogEntry> Logs);
    public sealed class GetAgentLogsFilesReq : IRequest<Result<GetAgentLogsFilesRes>>
    {
       public DateOnly StartDate { get; set; }
       public DateOnly EndDate { get; set; }
    }
    public sealed class GetAgentLogsFilesHandler : IRequestHandler<GetAgentLogsFilesReq, Result<GetAgentLogsFilesRes>>
    {
        ILogger<GetAgentLogsFilesHandler> _logger;
        private readonly LogReader _logReader;

        public GetAgentLogsFilesHandler(ILogger<GetAgentLogsFilesHandler> logger, LogReader logReader)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logReader = logReader ?? throw new ArgumentNullException(nameof(logReader));
        }

        public async Task<Result<GetAgentLogsFilesRes>> Handle(GetAgentLogsFilesReq request, CancellationToken cancellationToken)
        {
            try
            {
                var logEntries = _logReader.ReadLogsInRange(request.StartDate,request.EndDate);
                return Result<GetAgentLogsFilesRes>.Success("Data retrieved successfully").WithData(new GetAgentLogsFilesRes(logEntries.ToList()));
            }
            
            catch (FileNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return Result<GetAgentLogsFilesRes>.Failure("404", ex.Message).WithData(new GetAgentLogsFilesRes(new List<LogEntry>()));
            }
            catch (IOException ex)
            {
                _logger.LogInformation(ex.Message);
                return Result<GetAgentLogsFilesRes>.Failure("500", ex.Message).WithData(new GetAgentLogsFilesRes(new List<LogEntry>()));
            }
            catch (JsonException ex)
            {
                var errorDes = $"Error deserializing JSON. Error: {ex.Message}";
                _logger.LogError(errorDes);
                return Result<GetAgentLogsFilesRes>.Failure("500", errorDes).WithData(new GetAgentLogsFilesRes(new List<LogEntry>()));
            }
            catch (Exception ex)
            {
                var errorDes = $"Unexpected error: {ex.Message}";
                _logger.LogError(errorDes);
                return Result<GetAgentLogsFilesRes>.Failure("500", errorDes).WithData(new GetAgentLogsFilesRes(new List<LogEntry>()));
            }
        }

        

    }

}

