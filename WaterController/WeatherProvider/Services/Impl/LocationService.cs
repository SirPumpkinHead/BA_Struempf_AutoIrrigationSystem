#nullable enable
using System.Threading.Tasks;
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

        public LocationService(IHttpService httpService, ILogger<LocationService> logger)
        {
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<GeoLocation?> GetLocation(string entityId)
        {
            _logger.LogInformation("Loading geo location for {id}", entityId);

            if (entityId == "urn:ngsi-ld:Bed:001") // TODO remove this just for testing
            {
                return new GeoLocation
                {
                    Coordinates = new[] {"48.2082", "16.3738"}
                };
            }

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
    }
}