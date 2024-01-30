using Application.Common.Utilities;
using Domain.Common.Patterns;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.AgentLogs.Command.DeleteAgentLogsFiles
{
    public sealed record DeleteAgentLogsFilesRes(bool IsDeleted);
    public sealed class DeleteAgentLogsFilesReq : IRequest<Result<DeleteAgentLogsFilesRes>>
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
    public sealed class DeleteAgentLogsFilesHandler : IRequestHandler<DeleteAgentLogsFilesReq, Result<DeleteAgentLogsFilesRes>>
    {
        ILogger<DeleteAgentLogsFilesHandler> _logger;
        private readonly LogDeleter _logDeleter;

        public DeleteAgentLogsFilesHandler(ILogger<DeleteAgentLogsFilesHandler> logger, LogDeleter logDeleter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logDeleter = logDeleter ?? throw new ArgumentNullException(nameof(logDeleter));
        }

        public async Task<Result<DeleteAgentLogsFilesRes>> Handle(DeleteAgentLogsFilesReq request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _logDeleter.DeleteLogsInRange(request.StartDate, request.EndDate);
                if (result.All(x => x.IsDeleted == false))
                    return Result<DeleteAgentLogsFilesRes>.Failure("404", string.Concat(result.Select(s => $"{s.Message} , " )).TrimEnd(' ', ','), errorType: AgentErrorType.Business).WithData(new DeleteAgentLogsFilesRes(false));

                _logger.LogInformation($"Agent log file deleted for date range: {request.StartDate:yyyy-MM-dd} - {request.EndDate:yyyy-MM-dd}");
                return Result<DeleteAgentLogsFilesRes>.Success("Agent log file deleted successfully").WithData(new DeleteAgentLogsFilesRes(true));
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return Result<DeleteAgentLogsFilesRes>.Failure("404", ex.Message, errorType: AgentErrorType.Business).WithData(new DeleteAgentLogsFilesRes(false));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Result<DeleteAgentLogsFilesRes>.Failure("500", ex.Message).WithData(new DeleteAgentLogsFilesRes(false));
            }
        }


    }
}