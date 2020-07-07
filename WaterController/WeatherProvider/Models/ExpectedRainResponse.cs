using System.Text.Json.Serialization;

namespace WeatherProvider.Models
{
    public class ExpectedRainResponse
    {
        public string Id { get; set; }

        public string Type { get; set; }

        [JsonPropertyName("expRainVolume1h")]
        public Number ExpectedRainVolume1H { get; set; }
        
        [JsonPropertyName("expRainVolume4h")]
        public Number ExpectedRainVolume4H { get; set; }
    }

    public class Number
    {
        public string Type { get; set; } = "Number";

        public int Value { get; set; } = 0;
    }
}