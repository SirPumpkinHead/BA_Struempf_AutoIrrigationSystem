using System;
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
        private readonly IForecastService _forecastService;

        public WeatherConditionsController(ILogger<WeatherConditionsController> logger,
            ILocationService locationService, IWeatherService weatherService, IForecastService forecastService)
        {
            _logger = logger;
            _locationService = locationService;
            _weatherService = weatherService;
            _forecastService = forecastService;
        }

        [HttpPost("expected/rain/op/query")]
        public async Task<List<ExpectedRainResponse>> ExpectedRain(ExpectedRainRequest request)
        {
            var response = new List<ExpectedRainResponse>();
            var location = _locationService.GetConfiguredLocation();
            var weather = await _weatherService.GetWeatherForLocation(location);

            _logger.LogDebug("Request: {request}", JsonConvert.SerializeObject(request));

            foreach (var requestEntity in request.Entities)
            {
                _logger.LogInformation("Requesting rain for {id}", requestEntity.Id);
                response.Add(_forecastService.CalculateExpectedRain(requestEntity, weather));
            }

            return response;
        }

        [HttpGet("complete")]
        public async Task<WeatherForecast> GetForecastForConfigLocation()
        {
            var location = _locationService.GetConfiguredLocation();

            return await _weatherService.GetWeatherForLocation(location);
        }

        [HttpGet("expected/rain")]
        public async Task<ExpectedRainResponse> GetForecastForLocation([FromQuery] string lon = "16.37", [FromQuery] string lat = "48.21")
        {
            var location = new GeoLocation
            {
                Coordinates = new[] {lon, lat}
            };

            var weather = await _weatherService.GetWeatherForLocation(location);

            return _forecastService.CalculateExpectedRain(new Entity
            {
                Id = Guid.NewGuid().ToString()
            }, weather);
        }
    }
}