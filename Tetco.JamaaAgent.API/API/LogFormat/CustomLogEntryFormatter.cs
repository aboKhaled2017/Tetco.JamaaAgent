//using Application.Common.Models;
//using Serilog.Events;
//using Serilog.Formatting;
//using System.Text.Json;

//public class CustomLogEntryFormatter : ITextFormatter
//{
//    public void Format(LogEvent logEvent, TextWriter output)
//    {

//        LogEntry entry = new LogEntry
//        {
//            Timestamp = logEvent.Timestamp.DateTime,
//            Level = logEvent.Level.ToString(),
//            Message = logEvent.RenderMessage()
//        };


//        SaveToJsonFile(entry);
//        //// Serialize the LogEntry object to a string (you can use JSON or any other format)
//        //string logEntryString = SerializeLogEntry(entry);

//        //// Write the string to the output
//        //output.WriteLine(logEntryString);
//    }

//    //private string SerializeLogEntry(LogEntry entry)
//    //{
//    //    // Implement this method to convert the LogEntry object to a string
//    //    // For example, you can use JSON serialization:
//    //    // return JsonConvert.SerializeObject(entry);
//    //    return JsonConvert.SerializeObject(entry, Formatting.Indented);

//    //}


//    public async Task AppendObjectAsync<T>(T newObject)
//    {
//        string logFileName = $"log-{DateTime.Today:yyyyMMdd}.json";
//        string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
//        string logFilePath = Path.Combine(logDirectoryPath, logFileName);
//        // Ensures that the file is opened even if it's currently used by another process
//        using (var fileStream = new FileStream(logFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
//        {
//            // Read the existing content of the file
//            if (fileStream.Length > 0)
//            {
//                var existingData = await JsonSerializer.DeserializeAsync<JsonElement>(fileStream);

//                // If the file contains a JSON array, add the new object to it
//                if (existingData.ValueKind == JsonValueKind.Array)
//                {
//                    var dataArray = existingData.EnumerateArray().ToList();
//                    dataArray.Add(JsonSerializer.SerializeToElement(newObject));

//                    // Clear the file content before writing the updated JSON
//                    fileStream.SetLength(0);

//                    // Write the updated JSON array back to the file
//                    await JsonSerializer.SerializeAsync(fileStream, dataArray, new JsonSerializerOptions { WriteIndented = true });
//                }
//                else
//                {
//                    throw new InvalidOperationException("JSON structure in the file is not an array.");
//                }
//            }
//            else
//            {
//                // If the file is empty, write the new object as an array
//                var newArray = new[] { newObject };
//                await JsonSerializer.SerializeAsync(fileStream, newArray, new JsonSerializerOptions { WriteIndented = true });
//            }
//        }
//    }

//    void SaveToJsonFile(LogEntry logEntry)
//    {
//        try
//        {
//            string logFileName = $"log-{DateTime.Today:yyyyMMdd}.json";
//            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
//            string logFilePath = Path.Combine(logDirectoryPath, logFileName);

//            // Retry logic to handle file contention
//            const int maxRetries = 10; // Adjust the maximum number of retries as needed
//            const int retryDelayMilliseconds = 1000;

//            for (int retryCount = 0; retryCount < maxRetries; retryCount++)
//            {
//                try
//                {
//                    var options = new JsonSerializerOptions { WriteIndented = true };

//                    // Ensures that the file is opened even if it's currently used by another process
//                    using (var fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096, FileOptions.Asynchronous))
//                    {
//                         System.Text.Json.JsonSerializer.SerializeAsync(fileStream, logEntry, options);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    // Log error when an unexpected exception occurs
//                    Console.WriteLine($"Error saving JSON data to '{logFilePath}'. Error: {ex.Message}");

//                    // Handle the case where the file is in use
//                    Console.WriteLine($"File '{logFilePath}' is in use. Retrying after {retryDelayMilliseconds / 1000} seconds. Retry count: {retryCount + 1}");
//                    Thread.Sleep(retryDelayMilliseconds);
//                }
//            }

//            // Log information when retries are exhausted
//            Console.WriteLine($"Unable to save JSON data to '{logFilePath}' after {maxRetries} retries.");
//        }
//        catch (Exception ex)
//        {
//            // Log error for unexpected exceptions outside the retry loop
//            Console.WriteLine($"Unexpected error in SaveToJsonFile. Error: {ex.Message}");
//        }
//    }

//}