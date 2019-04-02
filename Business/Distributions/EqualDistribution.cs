using System;

namespace Business.Distributions
{
    public class EqualDistribution : IDistribution
    {
        Random random = new Random();

        public double Minimum { get; set; }

        public double Maximum { get; set; }

        public double Delta { get { return Maximum - Minimum; } }

        public EqualDistribution(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Gets the random value.
        /// </summary>
        /// <returns></returns>
        public double GetRandomValue()
        {
            var randomValue = random.NextDouble();

            return Minimum + randomValue * Delta;
        }
    }
}