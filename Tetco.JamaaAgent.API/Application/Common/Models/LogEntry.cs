using Newtonsoft.Json;

namespace Application.Common.Models
{
    public class LogEntry
    {
        [JsonProperty("@t")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("@mt")]
        public string MessageTemplate { get; set; }

        [JsonProperty("@l")]
        public string Level { get; set; }

        [JsonProperty("@x")]
        public string Exception { get; set; }

        [JsonProperty("@tr")]
        public string TraceId { get; set; }

        [JsonProperty("@sp")]
        public string SpanId { get; set; }

        [JsonProperty("ConnectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("TraceIdentifier")]
        public string TraceIdentifier { get; set; }

        [JsonProperty("EventId")]
        public EventId EventId { get; set; }

        [JsonProperty("SourceContext")]
        public string SourceContext { get; set; }

        [JsonProperty("RequestId")]
        public string RequestId { get; set; }

        [JsonProperty("RequestPath")]
        public string RequestPath { get; set; }

        [JsonProperty("Application")]
        public string ApplicationName { get; set; }

    }

    public class EventId
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }


}
