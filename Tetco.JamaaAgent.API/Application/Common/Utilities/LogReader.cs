using Application.Common.Models;
using Newtonsoft.Json;
namespace Application.Common.Utilities
{

    public class LogReader
    {

        public IEnumerable<LogEntry> ReadLogs(DateOnly date)
        {
            string logFileName = $"log-{date:yyyyMMdd}.json";
            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            string logFilePath = Path.Combine(logDirectoryPath, logFileName);

            if (!File.Exists(logFilePath))
            {
                throw new FileNotFoundException($"Log file not found for {date:yyyy-MM-dd}", logFileName);
            }

            return File.ReadLines(logFilePath)
                        .Select(line => JsonConvert.DeserializeObject<LogEntry>(line))
                        .Where(entry => entry != null);
        }
    }
}
