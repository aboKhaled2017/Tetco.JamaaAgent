using Application.Common.Models;
using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;

public class CustomLogEntryFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        LogEntry entry = new LogEntry
        {
            Timestamp = logEvent.Timestamp.DateTime,
            Level = logEvent.Level.ToString(),
            Message = logEvent.RenderMessage()
        };

        // Serialize the LogEntry object to a string (you can use JSON or any other format)
        string logEntryString = SerializeLogEntry(entry);

        // Write the string to the output
        output.WriteLine(logEntryString);
    }

    private string SerializeLogEntry(LogEntry entry)
    {
        // Implement this method to convert the LogEntry object to a string
        // For example, you can use JSON serialization:
        // return JsonConvert.SerializeObject(entry);
        return JsonConvert.SerializeObject(entry, Formatting.Indented);

    }
}