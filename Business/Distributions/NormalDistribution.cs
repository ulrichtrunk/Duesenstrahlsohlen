using System;

namespace Business.Distributions
{
    public class NormalDistribution : IDistribution
    {
        Random random = new Random();

        public double Mean { get; set; }
        public double StandardDerivation { get; set; }

        public NormalDistribution(double mean, double standardDerivation)
        {
            Mean = mean;
            StandardDerivation = standardDerivation;
        }

        /// <summary>
        /// Gets the random value.
        /// Source: http://stackoverflow.com/questions/218060/random-gaussian-variables
        /// </summary>
        /// <returns></returns>
        public double GetRandomValue()
        {
            double u1 = 1.0 - random.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2); //random normal(0,1)
            double randNormal = Mean + StandardDerivation * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }
    }
}