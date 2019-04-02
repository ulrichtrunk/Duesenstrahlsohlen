using Business.Distributions;
using Shared;
using Shared.Entities;
using System.Collections.Generic;

namespace Business.Plotters
{
    public class EqualDistributionPlotter : Plotter
    {
        public int DepthPixel { get; set; }
        private double minimum;
        private double maximum;

        public EqualDistributionPlotter(int width, 
            int height, 
            int offsetXPixel, 
            int offsetYPixel, 
            int sealingSlabRadius, 
            int borderX, 
            int borderY, 
            int depthPixel, 
            int minimum, 
            int maximum)
            : base(width, height, offsetXPixel, offsetYPixel, sealingSlabRadius, borderX, borderY)
        {
            DepthPixel = depthPixel;
            this.minimum = minimum;
            this.maximum = maximum;
        }


        /// <summary>
        /// Plots the sealing slabs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override IEnumerable<T> PlotSealingSlabs<T>()
        {
            var sealingSlabs = base.PlotSealingSlabs<T>();

            EqualDistribution equalDistribution = new EqualDistribution(minimum, maximum);

            foreach (var sealingSlab in sealingSlabs)
            {
                sealingSlab.OffsetX += (int)GetRandomDerivationSealingSlabPixel(equalDistribution);
                sealingSlab.OffsetY += (int)GetRandomDerivationSealingSlabPixel(equalDistribution);
            }

            return sealingSlabs;
        }


        /// <summary>
        /// Gets the random derivation sealing slab in pixel.
        /// </summary>
        /// <param name="equalDistribution">The equal distribution.</param>
        /// <returns></returns>
        private double GetRandomDerivationSealingSlabPixel(EqualDistribution equalDistribution)
        {
            var randomDerivationPercent = equalDistribution.GetRandomValue();
            var randomDerivationPixel = DepthPixel * randomDerivationPercent / 100;

            return randomDerivationPixel;
        }
    }
}