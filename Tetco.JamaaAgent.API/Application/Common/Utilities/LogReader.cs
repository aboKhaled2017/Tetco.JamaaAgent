using Application.Common.Models;
using Newtonsoft.Json;
using System.Text;
namespace Application.Common.Utilities
{

    public class LogReader
    {
        public List<LogEntry> ReadLogs(DateOnly date)
        {
            var logEntries = new List<LogEntry>();
            string logFileName = $"log-{date:yyyyMMdd}.json";
            string logContent = ReadLogFileContent(logFileName);

            // Split the file content into individual JSON entries
            string[] jsonEntries = logContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Extract desired properties from each JSON entry
            foreach (string jsonEntry in jsonEntries)
            {
                try
                {
                    dynamic logEntry = JsonConvert.DeserializeObject(jsonEntry);

                    string timestamp = logEntry["@t"];
                    string level = logEntry["@l"] ?? string.Empty;
                    string exception = logEntry["@mt"] ?? string.Empty;
                    string applicationName = logEntry["Application"] ?? string.Empty;

                    logEntries.Add(new LogEntry() { ApplicationName = applicationName, Exception = exception, Timestamp = timestamp, Level = level });
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                }
            }

            return logEntries;
        }

        public List<LogEntry> ReadLogsInRange(DateOnly startDate, DateOnly endDate)
        {
            var logEntries = new List<LogEntry>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                try
                {
                    logEntries.AddRange(ReadLogs(date));
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"Log file not found for {date:yyyy-MM-dd}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading log file for {date:yyyy-MM-dd}: {ex.Message}");
                }
            }

            return logEntries;
        }

        private string ReadLogFileContent(string logFileName)
        {
            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            string logFilePath = Path.Combine(logDirectoryPath, logFileName);

            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }

    }

}