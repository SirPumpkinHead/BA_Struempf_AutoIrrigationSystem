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
    }
}