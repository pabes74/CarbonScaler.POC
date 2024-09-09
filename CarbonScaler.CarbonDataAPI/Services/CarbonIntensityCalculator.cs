namespace CarbonScaler.CarbonDataAPI.Services
{
    public class CarbonIntensityCalculator
    {
        // Define the minimum and maximum carbon intensity values.
        private readonly double _minIntensity;
        private readonly double _maxIntensity;

        public CarbonIntensityCalculator(double minIntensity, double maxIntensity)
        {
            _minIntensity = minIntensity;
            _maxIntensity = maxIntensity;
        }

        // Method to recalculate carbon intensity to a grade between 0 and 10.
        public double RecalculateToGrade(double carbonIntensity)
        {
            if (carbonIntensity < _minIntensity || carbonIntensity > _maxIntensity)
            {
                throw new ArgumentOutOfRangeException(nameof(carbonIntensity), "Carbon intensity is out of range.");
            }

            // Normalize the carbon intensity to a value between 0 and 1
            double normalizedIntensity = (carbonIntensity - _minIntensity) / (_maxIntensity - _minIntensity);

            // Convert the normalized value to a grade between 0 and 10
            // 10 is the best score, so we subtract the normalized value from 1
            double grade = 10 * (1 - normalizedIntensity);

            return grade;
        }
    }
}
