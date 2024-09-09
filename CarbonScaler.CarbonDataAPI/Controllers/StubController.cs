using CarbonScaler.CarbonDataAPI.Models;
using CarbonScaler.CarbonDataAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarbonScaler.CarbonDataAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StubController : Controller
    {
        private readonly ILogger<StubController> _logger;
        public StubController(ILogger<StubController> logger)
        {
            _logger = logger;
        }

        // Interval for value change (5 minutes)
        private static readonly TimeSpan ChangeInterval = TimeSpan.FromMinutes(5);

        // Cache for storing the current value and the last generated time
        private static double _cachedValue;
        private static DateTime _lastGeneratedTime;

        // Random number generator
        private static readonly Random _random = new Random();

        public static double GetValue()
        {
            DateTime now = DateTime.UtcNow;

            // Check if the cached value needs to be updated
            if (now - _lastGeneratedTime > ChangeInterval)
            {
                // Update the cached value
                _cachedValue = GenerateRandomValue();
                _lastGeneratedTime = now;
            }

            return _cachedValue;
        }

        private static double GenerateRandomValue()
        {
            // Generate a random double between 0 and 10 with 2 decimal places
            return Math.Round(_random.NextDouble() * 10, 2);
        }


        [HttpGet(Name = "GetStubData")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting carbon intensity data from the Stub.");
            var stubValue = GetValue();
            var outputModel = new OutputModel { CarbonGrade = stubValue };
            _logger.LogInformation($"Current carbon grade {stubValue}");
            return Ok(outputModel);
        }
    }
}
