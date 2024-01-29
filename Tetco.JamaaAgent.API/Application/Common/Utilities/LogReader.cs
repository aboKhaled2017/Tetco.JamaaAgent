using Application.Common.Models;
using Newtonsoft.Json;
using System.Text;
namespace Application.Common.Utilities
{

    public class LogReader
    {
        public List<LogEntry> ReadLogs(DateOnly date)
        {

            string logFileName = $"log-{date:yyyyMMdd}.json";
            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            string logFilePath = Path.Combine(logDirectoryPath, logFileName);
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string content = streamReader.ReadToEnd();

                // Split the file content into individual JSON entries
                string[] jsonEntries = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                var logEntries = new List<LogEntry>();
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

                        logEntries.Add(new LogEntry() { ApplicationName = applicationName,Exception=exception,Timestamp=timestamp, Level=level});

                    }
                    catch (JsonReaderException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    }
                }

                return logEntries;
                
            }

        }

        
    }


    
}



