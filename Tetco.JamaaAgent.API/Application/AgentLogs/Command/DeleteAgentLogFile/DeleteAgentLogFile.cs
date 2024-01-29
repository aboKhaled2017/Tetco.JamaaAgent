using Domain.Common.Patterns;
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
        public DeleteAgentLogFileHandler(ILogger<DeleteAgentLogFileHandler> logger)
        {
            _logger = logger;
        }

        public async Task<Result<DeleteAgentLogFileRes>> Handle(DeleteAgentLogFileReq request, CancellationToken cancellationToken)
        {
            Task.CompletedTask.Wait();
            try
            {
                string logFileName = $"log-{request.Date:yyyy-MM-dd}.txt";
                string logFilePath = Path.Combine("logs", logFileName);

                if (File.Exists(logFilePath))
                {
                    File.Delete(logFilePath);
                    _logger.LogInformation($"Agent log file deleted for date: {request.Date:yyyy-MM-dd}");
                    return Result<DeleteAgentLogFileRes>.Success("Agent log file deleted successfully").WithData(new DeleteAgentLogFileRes(true));
                }
                else
                {
                    _logger.LogWarning($"Agent log file not found for date: {request.Date:yyyy-MM-dd}");
                    return Result<DeleteAgentLogFileRes>.Failure("404", "File not found").WithData(new DeleteAgentLogFileRes(false));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting agent log file.");
                return Result<DeleteAgentLogFileRes>.Failure("500", "Error while deleting agent log file.").WithData(new DeleteAgentLogFileRes(false));
            }
        }

    }
}