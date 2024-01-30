using Application.Common.Utilities;
using Domain.Common.Patterns;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.AgentLogs.Command.DeleteAgentLogsFile
{
    public sealed record DeleteAgentLogsFileRes(bool IsDeleted);
    public sealed class DeleteAgentLogsFileReq : IRequest<Result<DeleteAgentLogsFileRes>>
    {
        public DateOnly Date { get; set; }
    }
    public sealed class DeleteAgentLogsFileHandler : IRequestHandler<DeleteAgentLogsFileReq, Result<DeleteAgentLogsFileRes>>
    {
        ILogger<DeleteAgentLogsFileHandler> _logger;
        private readonly LogDeleter _logDeleter;

        public DeleteAgentLogsFileHandler(ILogger<DeleteAgentLogsFileHandler> logger, LogDeleter logDeleter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logDeleter = logDeleter ?? throw new ArgumentNullException(nameof(logDeleter));
        }

        public async Task<Result<DeleteAgentLogsFileRes>> Handle(DeleteAgentLogsFileReq request, CancellationToken cancellationToken)
        {
            try
            {
                _logDeleter.DeleteLog(request.Date);
                _logger.LogInformation($"Agent log file deleted for date: {request.Date:yyyy-MM-dd}");
                return Result<DeleteAgentLogsFileRes>.Success("Agent log file deleted successfully").WithData(new DeleteAgentLogsFileRes(true));
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return Result<DeleteAgentLogsFileRes>.Failure("404", ex.Message,errorType:AgentErrorType.Business).WithData(new DeleteAgentLogsFileRes(false));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Result<DeleteAgentLogsFileRes>.Failure("500", ex.Message).WithData(new DeleteAgentLogsFileRes(false));
            }
        }

        
    }
}