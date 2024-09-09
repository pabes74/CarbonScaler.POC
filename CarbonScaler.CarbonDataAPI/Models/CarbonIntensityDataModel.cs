namespace CarbonScaler.CarbonDataAPI.Models
{
    public class CarbonIntensityDataModel
    {
        public string zone { get; set; }
        public int carbonIntensity { get; set; }
        public DateTime datetime { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime xreatedAt { get; set; }
        public string emissionFactorType { get; set; }
        public bool isEstimated { get; set; }
        public string estimationMethod { get; set; }
    }
}
