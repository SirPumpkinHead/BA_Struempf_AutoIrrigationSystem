using RestSharp;

namespace WeatherProvider.Services
{
    public interface IHttpService
    {
        /// <summary>
        /// Instantiates a new rest client and set's required default headers and base path to context broker.
        /// </summary>
        /// <returns>The client</returns>
        public IRestClient GetContextClient();
    }
}