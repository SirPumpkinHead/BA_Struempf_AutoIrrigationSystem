#nullable enable
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using WeatherProvider.Exceptions;
using WeatherProvider.Models;

namespace WeatherProvider.Services.Impl
{
    public class LocationService : ILocationService
    {
        private readonly ILogger<LocationService> _logger;
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;

        public LocationService(IHttpService httpService, ILogger<LocationService> logger, IConfiguration configuration)
        {
            _httpService = httpService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<GeoLocation?> GetLocation(string entityId)
        {
            _logger.LogInformation("Loading geo location for {id}", entityId);

            var client = _httpService.GetContextClient();
            var request = new RestRequest($"/v2/entities/{entityId}/attrs/location/value", Method.GET);

            var response = await client.ExecuteAsync<GeoLocation>(request);

            if (!response.IsSuccessful)
            {
                throw new RequestFailedException(
                    $"({response.StatusCode}) - {response.StatusDescription} - {response.ErrorMessage}");
            }

            if (response.Data == null)
            {
                _logger.LogDebug("No location found for {id}", entityId);
                return null;
            }

            return response.Data;
        }

        public GeoLocation GetConfiguredLocation()
        {
            return new GeoLocation
            {
                Type = "Point",
                Coordinates = new[]
                {
                    _configuration.GetValue("LocationLong", "48.2082"),
                    _configuration.GetValue("LocationLat", "16.3738")
                }
            };
        }
    }
}