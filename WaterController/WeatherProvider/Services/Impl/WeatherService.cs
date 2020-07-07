using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using WeatherProvider.Exceptions;
using WeatherProvider.Models;

namespace WeatherProvider.Services.Impl
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(ILogger<WeatherService> logger, IHttpService httpService)
        {
            _logger = logger;
            _httpService = httpService;
        }

        public async Task<WeatherForecast> GetWeatherForLocation(GeoLocation location)
        {
            _logger.LogInformation("Loading weather for [{coords}]", string.Join(", ", location.Coordinates));

            var request = new RestRequest("data/2.5/onecall", Method.GET);
            request.AddQueryParameter("lat", location.Coordinates[0]);
            request.AddQueryParameter("lon", location.Coordinates[1]);

            var client = _httpService.GetWeatherClient();

            var response = await client.ExecuteAsync<WeatherForecast>(request);

            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        _logger.LogDebug("Trying to deserialize manually...");

                        return JsonConvert.DeserializeObject<WeatherForecast>(response.Content);
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning("Failed to deserialize manually", e);
                    }
                }

                _logger.LogWarning("Failed to get weather data: ({statusCode}) - {description} - {error}",
                    response.StatusCode, response.StatusDescription, response.ErrorMessage);

                throw new RequestFailedException(
                    $"Failed to load weather data: ({response.StatusCode}) {response.ErrorMessage}");
            }

            _logger.LogDebug("Loaded weather data: ({statusCode}) - {description}",
                response.StatusCode, response.StatusDescription);

            return response.Data;
        }
    }
}