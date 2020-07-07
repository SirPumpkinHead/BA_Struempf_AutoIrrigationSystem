using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WeatherProvider.Models
{
    public class ExpectedRainRequest
    {
        [JsonPropertyName("entities")]
        public List<Entity> Entities { get; set; } = new List<Entity>();

        [JsonPropertyName("attrs")]
        public List<string> Attributes { get; set; }

        public string Test { get; set; }
    }

    public class Entity
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string IsPattern { get; set; }
    }
}