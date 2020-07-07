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

        [JsonPropertyName("expRainVolume8h")]
        public Number ExpectedRainVolume8H { get; set; }

        [JsonPropertyName("expRainVolume1D")]
        public Number ExpectedRainVolume1D { get; set; }
        
        [JsonPropertyName("expRainVolume2D")]
        public Number ExpectedRainVolume2D { get; set; }
    }

    public class Number
    {
        public string Type { get; set; } = "Number";

        public double Value { get; set; } = 0;
    }
}