using Microsoft.Extensions.Configuration;
using RestSharp;

namespace WeatherProvider.Services.Impl
{
    public class HttpService : IHttpService
    {
        private readonly IConfiguration _configuration;

        public HttpService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IRestClient GetContextClient()
        {
            var contextBrokerPath = _configuration.GetValue<string>("ContextBrokerPath");
            var serviceHeader = _configuration.GetValue<string>("FiwareService");
            var servicePathHeader = _configuration.GetValue<string>("FiwareServicePath");

            var client = new RestClient(contextBrokerPath);
            client.AddDefaultHeader("fiware-service", serviceHeader);
            client.AddDefaultHeader("fiware-servicepath", servicePathHeader);

            return client;
        }

        public RestClient GetWeatherClient()
        {
            var apiKey = _configuration.GetValue<string>("OpenWeatherApiKey");
            var basePath = _configuration.GetValue("OpenWeatherBasePath", "https://api.openweathermap.org");

            var client = new RestClient(basePath);

            client.AddDefaultQueryParameter("appid", apiKey);

            return client;
        }
    }
}