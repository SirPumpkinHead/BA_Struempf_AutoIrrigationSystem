using System.Threading.Tasks;
using WeatherProvider.Models;

namespace WeatherProvider.Services
{
    public interface IWeatherService
    {
        public Task<WeatherForecast> GetWeatherForLocation(GeoLocation location);
    }
}