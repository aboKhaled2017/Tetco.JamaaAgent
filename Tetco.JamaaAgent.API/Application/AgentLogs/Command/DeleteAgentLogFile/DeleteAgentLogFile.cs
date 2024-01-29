using Application.Common.Utilities;
using Domain.Common.Patterns;
using Domain.Common.Settings;
using Microsoft.Extensions.Logging;

namespace Application.AgentLogs.Command.DeleteAgentLogFile
{
    public sealed record DeleteAgentLogFileRes(bool IsDeleted);
    public sealed class DeleteAgentLogFileReq : IRequest<Result<DeleteAgentLogFileRes>>
    {
        public DateOnly Date { get; set; }
    }
    public sealed class DeleteAgentLogFileHandler : IRequestHandler<DeleteAgentLogFileReq, Result<DeleteAgentLogFileRes>>
    {
        ILogger<DeleteAgentLogFileHandler> _logger;
        private readonly LogDeleter _logDeleter;

        public DeleteAgentLogFileHandler(ILogger<DeleteAgentLogFileHandler> logger, LogDeleter logDeleter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logDeleter = logDeleter ?? throw new ArgumentNullException(nameof(logDeleter));
        }

        public async Task<Result<DeleteAgentLogFileRes>> Handle(DeleteAgentLogFileReq request, CancellationToken cancellationToken)
        {
            try
            {
                _logDeleter.DeleteLog(request.Date);
                _logger.LogInformation($"Agent log file deleted for date: {request.Date:yyyy-MM-dd}");
                return Result<DeleteAgentLogFileRes>.Success("Agent log file deleted successfully").WithData(new DeleteAgentLogFileRes(true));
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return Result<DeleteAgentLogFileRes>.Failure("404", ex.Message).WithData(new DeleteAgentLogFileRes(false));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Result<DeleteAgentLogFileRes>.Failure("500", ex.Message).WithData(new DeleteAgentLogFileRes(false));
            }
        }

        
    }
}