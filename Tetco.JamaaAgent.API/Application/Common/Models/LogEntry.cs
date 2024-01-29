using Newtonsoft.Json;

namespace Application.Common.Models
{
    public class LogEntry
    {
        [JsonProperty("@t")]
        public string Timestamp { get; set; }

        [JsonProperty("@l")]
        public string Level { get; set; }

        [JsonProperty("@mt")]
        public string Exception { get; set; }

        [JsonProperty("Application")]
        public string ApplicationName { get; set; }

    }

}
