using WeatherProvider.Models;

namespace WeatherProvider.Services
{
    public interface IForecastService
    {
        /// <summary>
        /// Calculates the expected rain volume for a location.
        /// </summary>
        /// <param name="requestEntity"></param>
        /// <param name="weatherForecast"></param>
        /// <returns></returns>
        public ExpectedRainResponse CalculateExpectedRain(Entity requestEntity, WeatherForecast weatherForecast);
    }
}