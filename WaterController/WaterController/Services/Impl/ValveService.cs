using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WaterController.Services.Impl
{
    public class ValveService : IValveService
    {
        private readonly ILogger<ValveService> _logger;

        public ValveService(ILogger<ValveService> logger)
        {
            _logger = logger;
        }

        public async Task OpenValveIfRequired()
        {
            _logger.LogInformation("Checking if vale should be opened...");
        }
    }
}