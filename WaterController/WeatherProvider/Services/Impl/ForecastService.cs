using WeatherProvider.Models;

namespace WeatherProvider.Services.Impl
{
    public class ForecastService : IForecastService
    {
        public ExpectedRainResponse CalculateExpectedRain(Entity requestEntity, WeatherForecast weather)
        {
            var response = new ExpectedRainResponse
            {
                Id = requestEntity.Id,
                Type = requestEntity.Type,
                ExpectedRainVolume1H = new Number(),
                ExpectedRainVolume4H = new Number(),
                ExpectedRainVolume8H = new Number(),
                ExpectedRainVolume1D = new Number()
            };

            response.ExpectedRainVolume1H.Value = weather.Hourly[0].Rain?.The1H ?? 0;

            for (var i = 0; i < 4 && i < weather.Hourly.Length; i++)
            {
                response.ExpectedRainVolume4H.Value = weather.Hourly[i].Rain?.The1H ?? 0;
            }

            for (var i = 0; i < 8 && i < weather.Hourly.Length; i++)
            {
                response.ExpectedRainVolume4H.Value = weather.Hourly[i].Rain?.The1H ?? 0;
            }

            response.ExpectedRainVolume1D.Value = weather.Daily[0].Rain;

            for (var i = 0; i < 2 && i < weather.Daily.Length; i++)
            {
                response.ExpectedRainVolume2D.Value = weather.Daily[i].Rain;
            }

            return response;
        }
    }
}