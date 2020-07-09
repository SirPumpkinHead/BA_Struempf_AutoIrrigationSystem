using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace WaterController.Services.Impl
{
    public class ValveService : IValveService
    {
        // TODO make ids configurable
        private const string ValveId = "urn:ngsi-ld:Valve:001";
        private const string BedId = "urn:ngsi-ld:Bed:001";
        private const double MinimumRainIn1H = 0.2;
        private const double MinimumRainIn2H = 0.5;
        private const double MinimumRainIn1D = 1.5;
        private const double MinimumRainIn2D = 5;

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
                .Select(l => l.SufficientSufficientMoisture?.Value)
                .ToList();

            _logger.LogDebug("Got [{@levels}] moisture levels", levels);

            if (!levels.Contains("not_sufficient"))
            {
                _logger.LogInformation("Flower bed {id} is sufficiently watered", BedId);
                return;
            }

            var flowerBed = await _entityService.GetFlowerBed(BedId);

            if (flowerBed.ExpRainVolume1H.Value > MinimumRainIn1H)
            {
                _logger.LogInformation("Expecting {volume} rain in 1 hour - not watering",
                    flowerBed.ExpRainVolume1H.Value);
                return;
            }

            if (flowerBed.ExpRainVolume2H.Value > MinimumRainIn2H)
            {
                _logger.LogInformation("Expecting {volume} rain in 2 hours - not watering",
                    flowerBed.ExpRainVolume2H.Value);
                return;
            }

            if (flowerBed.ExpRainVolume1D.Value < MinimumRainIn1D)
            {
                _logger.LogInformation("Expecting {volume} rain in 1 day - watering for 15 min",
                    flowerBed.ExpRainVolume1D.Value);

                await _entityService.SendCommand(ValveId, "open");

                BackgroundJob.Schedule<IEntityService>(s => s.SendCommand(ValveId, "close"), TimeSpan.FromMinutes(15));
                return;
            }

            if (flowerBed.ExpRainVolume2D.Value < MinimumRainIn2D)
            {
                _logger.LogInformation("Expecting {volume} rain in 2 days - watering for 25 min",
                    flowerBed.ExpRainVolume2D.Value);

                await _entityService.SendCommand(ValveId, "open");

                BackgroundJob.Schedule<IEntityService>(s => s.SendCommand(ValveId, "close"), TimeSpan.FromMinutes(25));
            }

            _logger.LogInformation("No action taken.");
        }
    }
}