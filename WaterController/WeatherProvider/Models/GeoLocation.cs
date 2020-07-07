namespace WeatherProvider.Models
{
    public class GeoLocation
    {
        public string Type { get; set; } = "Point";
        public string[] Coordinates { get; set; }
    }
}