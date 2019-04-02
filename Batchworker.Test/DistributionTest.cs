using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Batchworker.Test
{
    [TestClass]
    public class DistributionTest
    {
        [TestMethod]
        public void NormalDistributionTest()
        {
            var normalDistribution = new Business.Distributions.NormalDistribution(0, 1);

            uint zeroToOne = 0;
            uint oneToTwo = 0;
            uint twoToThree = 0;
            uint threeToFour = 0;
            uint fourToFive = 0;

            int iterations = 100000;

            for (int i = 0; i < iterations; i++)
            {
                var value = normalDistribution.GetRandomValue();

                if (Math.Abs(value) < 1) { zeroToOne++; }
                else if (Math.Abs(value) < 2) { oneToTwo++; }
                else if (Math.Abs(value) < 3) { twoToThree++; }
                else if (Math.Abs(value) < 4) { threeToFour++; }
                else if (Math.Abs(value) < 5) { fourToFive++; }
            }

            System.Diagnostics.Debug.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", zeroToOne , oneToTwo , twoToThree , threeToFour, fourToFive);
        }


        [TestMethod]
        public void EqualDistributionTest()
        {
            var equalDistribution = new Business.Distributions.EqualDistribution(-5, 5);

            uint zeroToOne = 0;
            uint oneToTwo = 0;
            uint twoToThree = 0;
            uint threeToFour = 0;
            uint fourToFive = 0;

            int iterations = 10000000;

            for (int i = 0; i < iterations; i++)
            {
                var value = equalDistribution.GetRandomValue();

                if (Math.Abs(value) < 1) { zeroToOne++; }
                else if (Math.Abs(value) < 2) { oneToTwo++; }
                else if (Math.Abs(value) < 3) { twoToThree++; }
                else if (Math.Abs(value) < 4) { threeToFour++; }
                else if (Math.Abs(value) < 5) { fourToFive++; }
            }

            System.Diagnostics.Debug.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", zeroToOne, oneToTwo, twoToThree, threeToFour, fourToFive);
        }

        [TestMethod]
        public void NormalDistributionDifferentValuesTest()
        {
            var normalDistribution = new Business.Distributions.NormalDistribution(0, 1);
            
            int iterations = 100000;

            var randomValues = new System.Collections.Generic.Dictionary<double, bool>();

            for (int i = 0; i < iterations; i++)
            {
                randomValues.Add(normalDistribution.GetRandomValue(), true);
            }
        }
    }
}