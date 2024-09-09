using CarbonScaler.CarbonDataAPI.Services;
using Microsoft.AspNetCore.Mvc;
using CarbonScaler.CarbonDataAPI.Models;
using System.Xml;

namespace CarbonScaler.CarbonDataAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElectricityMapController : Controller
    {
        private readonly ElectricityMapService _electricityMapService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ElectricityMapController> _logger;

        public ElectricityMapController(ElectricityMapService electricityMapService, ILogger<ElectricityMapController> logger)
        {
            _electricityMapService = electricityMapService;
            _logger = logger;
        }

        [HttpGet(Name = "GetCarbonData")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Getting carbon intensity data from the ElectricityMap API.");
                var carbonGrade = await _electricityMapService.GetCarbonIntensityDataAsync();
                var outputModel = new OutputModel { CarbonGrade = carbonGrade };
                _logger.LogInformation($"Current carbon grade {carbonGrade}" );
                return Ok(outputModel);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}



