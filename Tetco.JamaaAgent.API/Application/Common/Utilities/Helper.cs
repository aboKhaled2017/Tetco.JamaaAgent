//using Application.AgentLogs.Command.DeleteAgentLogFile;
//using Application.AgentLogs.Queries.GetAgentLogs;
//using Application.Common.Models;
//using Domain.Common.Patterns;
//using Microsoft.Extensions.Logging;

//namespace Application.Common.Utilities
//{



//    public class Helper
//    {
//        public Result<GetAgentLogsRes> GetLogsByDate(DateOnly date, ILogger _logger)
//        {
//            string logFileName = $"log-{date:yyyyMMdd}.json";
//            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
//            string logFilePath = Path.Combine(logDirectoryPath, logFileName);

//            if (!File.Exists(logFilePath))
//            {
//                _logger.LogInformation($"Log file not found for {date:yyyy-MM-dd}");
//                return Result<GetAgentLogsRes>.Failure("404", "File not found").WithData(new GetAgentLogsRes(new List<LogEntry>()));
//            }

//            var logEntries = new List<LogEntry>();

//            try
//            {
//                var logLines = System.IO.File.ReadAllLines(logFilePath);
//                foreach (var line in logLines)
//                {
//                    var logEntry = JsonConvert.DeserializeObject<LogEntry>(line);
//                    if (logEntry != null)
//                    {
//                        logEntries.Add(logEntry);
//                    }
//                }
//            }
//            catch (IOException ex) when (IsFileInUse(ex))
//            {
//                var errorDes = $"File '{logFilePath}' is in use.";
//                _logger.LogError(errorDes);
//                return Result<GetAgentLogsRes>.Failure("500", errorDes).WithData(new GetAgentLogsRes(logEntries));

//            }
//            catch (JsonException jsonEx)
//            {
//                var errorDes = $"Error deserializing JSON file '{logFilePath}'. Error: {jsonEx.Message}";
//                _logger.LogError(errorDes);
//                return Result<GetAgentLogsRes>.Failure("500", errorDes).WithData(new GetAgentLogsRes(logEntries));

//            }
//            catch (Exception ex)
//            {
//                var errorDes = $"Unexpected error reading JSON file '{logFilePath}'. Error: {ex.Message}";
//                _logger.LogError(errorDes);
//                return Result<GetAgentLogsRes>.Failure("500", errorDes).WithData(new GetAgentLogsRes(logEntries));

//            }

//            return Result<GetAgentLogsRes>.Success("Data retrieved successfully").WithData(new GetAgentLogsRes(logEntries));
//        }

//        private bool IsFileInUse(IOException ex)
//        {
//            throw new NotImplementedException();
//        }

//        public Result<DeleteAgentLogFileRes> DeleteFileByDate(DateOnly date, ILogger _logger)
//        {
//            string logFileName = $"log-{date:yyyy-MM-dd}.txt";
//            string logFilePath = Path.Combine("logs", logFileName);

//            if (File.Exists(logFilePath))
//            {
//                File.Delete(logFilePath);
//                _logger.LogInformation($"Agent log file deleted for date: {date:yyyy-MM-dd}");
//                return Result<DeleteAgentLogFileRes>.Success("Agent log file deleted successfully").WithData(new DeleteAgentLogFileRes(true));
//            }
//            else
//            {
//                _logger.LogWarning($"Agent log file not found for date: {date:yyyy-MM-dd}");
//                return Result<DeleteAgentLogFileRes>.Failure("404", "File not found").WithData(new DeleteAgentLogFileRes(false));
//            }
//        }

//    }
//}
