using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherProvider.Models;

namespace WeatherProvider.Controllers
{
    public class WeatherConditionsController : BaseController
    {
        private readonly ILogger<WeatherConditionsController> _logger;

        public WeatherConditionsController(ILogger<WeatherConditionsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("expected/rain/op/query")]
        public async Task<object> ExpectedRain(ExpectedRainRequest request)
        {
            var response = new List<ExpectedRainResponse>();
            foreach (var requestEntity in request.Entities)
            {
                _logger.LogInformation($"Requesting rain for {requestEntity.Id}");
                response.Add(new ExpectedRainResponse
                {
                    Id = requestEntity.Id,
                    Type = requestEntity.Type,
                    ExpectedRainVolume1H = new Number
                    {
                        Value = 100
                    },
                    ExpectedRainVolume4H = new Number
                    {
                        Value = 200
                    }
                });
            }

            return Ok(response);
        }
    }
}