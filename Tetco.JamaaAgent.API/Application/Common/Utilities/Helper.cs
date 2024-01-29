using Application.AgentLogs.Queries.GetAgentLogs;
using Application.Common.Models;
using Domain.Common.Patterns;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Application.Common.Utilities
{
    public class Helper
    {
        public LogEntry ParseLogLine(string logLine)
        {
            // Define the expected date and time format in the log timestamp
            string dateFormat = "yyyy-MM-dd HH:mm:ss";

            // Split the log line by space into timestamp, log level, and message
            string[] parts = logLine.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
            {
                // If the log line does not have exactly 3 parts, it's not a valid log entry
                return null;
            }

            if (!DateTime.TryParseExact(parts[0], dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp))
            {
                // If the timestamp cannot be parsed, it's not a valid log entry
                return null;
            }

            string logLevel = parts[1].Trim('[', ']'); // Remove square brackets from log level
            string message = parts[2].Trim(); // Trim leading and trailing whitespace from the message

            // Create and return a LogEntry object
            return new LogEntry
            {
                Timestamp = timestamp,
                Level = logLevel,
                Message = message
            };
        }


        public Result<GetAgentLogsRes> GetLogsByDate(DateOnly Date, ILogger _logger)
        {
            string logFileName = $"log-{Date:yyyy-MM-dd}.txt";
            string logFilePath = Path.Combine("logs", logFileName);

            if (!File.Exists(logFilePath))
            {
                _logger.LogInformation($"Log file not found for {Date:yyyy-MM-dd}");
                return Result<GetAgentLogsRes>.Failure("404", "File not found").WithData(new GetAgentLogsRes(new List<LogEntry>()));
            }

            var logLines = File.ReadAllLines(logFilePath);
            var logEntries = new List<LogEntry>();

            foreach (var logLine in logLines)
            {
                LogEntry logEntry = ParseLogLine(logLine);
                if (logEntry != null)
                {
                    logEntries.Add(logEntry);
                }
            }
            return Result<GetAgentLogsRes>.Success("Data retrieved successfully").WithData(new GetAgentLogsRes(logEntries));
        }
    }
}
