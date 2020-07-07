using System.Text.Json.Serialization;

namespace WeatherProvider.Models
{
    /// <summary>
    /// Contains the expected rain volume in mm for various time periods.
    /// </summary>
    public class ExpectedRainResponse
    {
        public string Id { get; set; }

        public string Type { get; set; }

        [JsonPropertyName("expRainVolume1h")]
        public Number ExpectedRainVolume1H { get; set; }

        [JsonPropertyName("expRainVolume2h")]
        public Number ExpectedRainVolume2H { get; set; }

        [JsonPropertyName("expRainVolume4h")]
        public Number ExpectedRainVolume4H { get; set; }

        [JsonPropertyName("expRainVolume8h")]
        public Number ExpectedRainVolume8H { get; set; }

        [JsonPropertyName("expRainVolume16h")]
        public Number ExpectedRainVolume16H { get; set; }

        [JsonPropertyName("expRainVolume1d")]
        public Number ExpectedRainVolume1D { get; set; }

        [JsonPropertyName("expRainVolume2d")]
        public Number ExpectedRainVolume2D { get; set; }

        [JsonPropertyName("expRainVolume3d")]
        public Number ExpectedRainVolume3D { get; set; }

        [JsonPropertyName("expRainVolume4d")]
        public Number ExpectedRainVolume4D { get; set; }
        
        [JsonPropertyName("expRainVolume5d")]
        public Number ExpectedRainVolume5D { get; set; }
        
        [JsonPropertyName("expRainVolume6d")]
        public Number ExpectedRainVolume6D { get; set; }
        
        [JsonPropertyName("expRainVolume7d")]
        public Number ExpectedRainVolume7D { get; set; }
    }

    public class Number
    {
        public string Type { get; set; } = "Number";

        public double Value { get; set; } = 0;
    }
}