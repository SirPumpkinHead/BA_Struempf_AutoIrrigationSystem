using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WaterController.Services.Impl
{
    public class ValveService : IValveService
    {
        // TODO make ids configurable
        private const string ValveId = "urn:ngsi-ld:Valve:001";
        private const string BedId = "urn:ngsi-ld:Bed:001";

        private readonly ILogger<ValveService> _logger;
        private readonly IEntityService _entityService;


        public ValveService(ILogger<ValveService> logger, IEntityService entityService)
        {
            _logger = logger;
            _entityService = entityService;
        }

        public async Task OpenValveIfRequired()
        {
            _logger.LogInformation("Checking if vale should be opened...");

            var levels = (await _entityService.GetMoistureLevels(BedId))
                .Select(l => l.Moisture.GetValue())
                .ToList();

            _logger.LogDebug("Got [{@levels}] moisture levels", levels);
        }
    }
}