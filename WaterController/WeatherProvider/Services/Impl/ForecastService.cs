using WeatherProvider.Models;

namespace WeatherProvider.Services.Impl
{
    public class ForecastService : IForecastService
    {
        public ExpectedRainResponse CalculateExpectedRain(Entity requestEntity, WeatherForecast weather)
        {
            return new ExpectedRainResponse
            {
                Id = requestEntity.Id,
                Type = requestEntity.Type,
                ExpectedRainVolume1H = GetExpectedRainInHours(1, weather),
                ExpectedRainVolume2H = GetExpectedRainInHours(2, weather),
                ExpectedRainVolume4H = GetExpectedRainInHours(4, weather),
                ExpectedRainVolume8H = GetExpectedRainInHours(8, weather),
                ExpectedRainVolume16H = GetExpectedRainInHours(16, weather),
                ExpectedRainVolume1D = GetExpectedRainInDays(1, weather),
                ExpectedRainVolume2D = GetExpectedRainInDays(2, weather),
                ExpectedRainVolume3D = GetExpectedRainInDays(3, weather),
                ExpectedRainVolume4D = GetExpectedRainInDays(4, weather),
                ExpectedRainVolume5D = GetExpectedRainInDays(5, weather),
                ExpectedRainVolume6D = GetExpectedRainInDays(6, weather),
                ExpectedRainVolume7D = GetExpectedRainInDays(7, weather)
            };
        }

        private static Number GetExpectedRainInHours(int hours, WeatherForecast weather)
        {
            var number = new Number();

            for (var i = 0; i < hours && i < weather.Hourly.Length; i++)
            {
                number.Value += weather.Hourly[i].Rain?.The1H ?? 0;
            }

            return number;
        }

        private static Number GetExpectedRainInDays(int days, WeatherForecast weather)
        {
            var number = new Number();

            for (var i = 0; i < days && i < weather.Daily.Length; i++)
            {
                number.Value += weather.Daily[i].Rain;
            }

            return number;
        }
    }
}