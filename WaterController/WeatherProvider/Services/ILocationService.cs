#nullable enable
using System.Threading.Tasks;
using WeatherProvider.Exceptions;
using WeatherProvider.Models;

namespace WeatherProvider.Services
{
    public interface ILocationService
    {
        /// <summary>
        /// Loads the geo location for a given entity.
        /// </summary>
        /// <param name="entityId">The entity that should have a geo location</param>
        /// <exception cref="RequestFailedException">If the request returned an error code</exception>
        /// <returns>The geo location of the given entity</returns>
        public Task<GeoLocation?> GetLocation(string entityId);

        /// <summary>
        /// Gets the configured location for getting weather data.
        /// Default value is in the city center of Vienna (48.2082N, 16.3738E). 
        /// </summary>
        /// <returns></returns>
        public GeoLocation GetConfiguredLocation();
    }
}