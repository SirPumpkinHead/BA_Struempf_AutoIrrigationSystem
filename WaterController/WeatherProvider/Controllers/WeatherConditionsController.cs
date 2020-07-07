using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherProvider.Models;
using WeatherProvider.Services;

namespace WeatherProvider.Controllers
{
    public class WeatherConditionsController : BaseController
    {
        private readonly ILogger<WeatherConditionsController> _logger;
        private readonly ILocationService _locationService;
        private readonly IWeatherService _weatherService;

        public WeatherConditionsController(ILogger<WeatherConditionsController> logger,
            ILocationService locationService, IWeatherService weatherService)
        {
            _logger = logger;
            _locationService = locationService;
            _weatherService = weatherService;
        }

        [HttpPost("expected/rain/op/query")]
        public async Task<object> ExpectedRain(ExpectedRainRequest request)
        {
            var response = new List<ExpectedRainResponse>();

            _logger.LogDebug("Request: {request}", JsonConvert.SerializeObject(request));

            foreach (var requestEntity in request.Entities)
            {
                _logger.LogInformation("Requesting rain for {id}", requestEntity.Id);
                response.Add(new ExpectedRainResponse
                {
                    Id = requestEntity.Id,
                    Type = requestEntity.Type,
                    ExpectedRainVolume1H = new Number
                    {
                        Value = 100
                    },
                    ExpectedRainVolume4H = new Number
                    {
                        Value = 200
                    }
                });

                var location = await _locationService.GetLocation(requestEntity.Id);

                _logger.LogInformation("Location for {id} is {@location}", requestEntity.Id,
                    location?.Coordinates ?? new[] {"-1", "-1"});
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<WeatherForecast> GetForecastForConfigLocation()
        {
            var location = _locationService.GetConfiguredLocation();

            return await _weatherService.GetWeatherForLocation(location);
        }
    }
}