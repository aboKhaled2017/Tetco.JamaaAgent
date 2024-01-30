using Application.Common.Models;
using Application.Common.Utilities;
using Domain.Common.Patterns;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.AgentLogs.Queries.GetAgentLogsFile
{
    public sealed record GetAgentLogsFileRes(IEnumerable<LogEntry> Logs);
    public sealed class GetAgentLogsFileReq : IRequest<Result<GetAgentLogsFileRes>>
    {
        public DateOnly Date { get; set; }
    }
    public sealed class GetAgentLogsFileHandler : IRequestHandler<GetAgentLogsFileReq, Result<GetAgentLogsFileRes>>
    {
        ILogger<GetAgentLogsFileHandler> _logger;
        private readonly LogReader _logReader;

        public GetAgentLogsFileHandler(ILogger<GetAgentLogsFileHandler> logger, LogReader logReader)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logReader = logReader ?? throw new ArgumentNullException(nameof(logReader));
        }

        public async Task<Result<GetAgentLogsFileRes>> Handle(GetAgentLogsFileReq request, CancellationToken cancellationToken)
        {
            try
            {
                var logEntries = _logReader.ReadLogs(request.Date);
                return Result<GetAgentLogsFileRes>.Success("Data retrieved successfully").WithData(new GetAgentLogsFileRes(logEntries.ToList()));
            }

            catch (FileNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return Result<GetAgentLogsFileRes>.Failure("404", ex.Message, errorType: AgentErrorType.Business).WithData(new GetAgentLogsFileRes(new List<LogEntry>()));
            }
            catch (IOException ex)
            {
                _logger.LogInformation(ex.Message);
                return Result<GetAgentLogsFileRes>.Failure("500", ex.Message).WithData(new GetAgentLogsFileRes(new List<LogEntry>()));
            }
            catch (JsonException ex)
            {
                var errorDes = $"Error deserializing JSON. Error: {ex.Message}";
                _logger.LogError(errorDes);
                return Result<GetAgentLogsFileRes>.Failure("500", errorDes).WithData(new GetAgentLogsFileRes(new List<LogEntry>()));
            }
            catch (Exception ex)
            {
                var errorDes = $"Unexpected error: {ex.Message}";
                _logger.LogError(errorDes);
                return Result<GetAgentLogsFileRes>.Failure("500", errorDes).WithData(new GetAgentLogsFileRes(new List<LogEntry>()));
            }
        }



    }

}

