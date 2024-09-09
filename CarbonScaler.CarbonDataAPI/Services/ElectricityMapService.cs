
using CarbonScaler.CarbonDataAPI.Models;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace CarbonScaler.CarbonDataAPI.Services
{
    public class ElectricityMapService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _carbonIntensityEndpoint;
        private readonly string _authToken;
        private readonly string _zone;
        

        public ElectricityMapService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _carbonIntensityEndpoint = configuration["ElectricityMapService:CarbonIntensityEndpoint"];
            _baseAddress = configuration["ElectricityMapService:BaseAddress"];
            _authToken = configuration["ElectricityMapService:AuthToken"];
            _zone = configuration["ElectricityMapService:Zone"];

            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"auth-token {_authToken}");
        }

        // Add methods to interact with the API
        public async Task<double> GetCarbonIntensityDataAsync()
        {
            var response = await _httpClient.GetAsync($"{_carbonIntensityEndpoint}?zone={_zone}");
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();

            var carbonResult = JsonSerializer.Deserialize<CarbonIntensityDataModel>(jsonString)
                   ?? throw new JsonException("Failed to deserialize CarbonIntensityDataModel.");
            //Set static min and max values for carbon intensity (0 to 1500 according to electricityMap API)
            var carbonCalculator = new CarbonIntensityCalculator(0, 1500);
            var carbonGrade = carbonCalculator.RecalculateToGrade(carbonResult.carbonIntensity);
            return carbonGrade;
        }
    }
}


