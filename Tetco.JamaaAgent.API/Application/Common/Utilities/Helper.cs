using Application.AgentLogs.Queries.GetAgentLogs;
using Application.Common.Models;
using Domain.Common.Patterns;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Application.Common.Utilities
{
    public class Helper
    {
        public Result<GetAgentLogsRes> GetLogsByDate(DateOnly date, ILogger _logger)
        {
            string logFileName = $"log-{date:yyyy-MM-dd}.txt";
            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            string logFilePath = Path.Combine(logDirectoryPath, logFileName);

            if (!File.Exists(logFilePath))
            {
                _logger.LogInformation($"Log file not found for {date:yyyy-MM-dd}");
                return Result<GetAgentLogsRes>.Failure("404", "File not found").WithData(new GetAgentLogsRes(new List<LogEntry>()));
            }

            var logEntries = new List<LogEntry>();
            var logLines = System.IO.File.ReadAllLines(logFilePath, Encoding.UTF8);

            foreach (var line in logLines)
            {
                try
                {
                    var entry = JsonConvert.DeserializeObject<LogEntry>(line);
                    if (entry != null)
                    {
                        logEntries.Add(entry);
                    }
                }
                catch (JsonException jsonEx)
                {
                    // Handle the case where a line is not a valid JSON
                    Console.WriteLine($"Error parsing log line: {line}. Error: {jsonEx.Message}");
                }
            }

            return Result<GetAgentLogsRes>.Success("Data retrieved successfully").WithData(new GetAgentLogsRes(logEntries));
        }
    }
}
