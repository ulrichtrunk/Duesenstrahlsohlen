using Business.Distributions;
using Shared;
using Shared.Entities;
using System.Collections.Generic;

namespace Business.Plotters
{
    public class NormalDistributionPlotter : Plotter
    {
        public int DepthPixel { get; set; }

        private double standardDerivationOffset;
        private double standardDerivationRadius;
        private double standardDerivationDrillingPoint;

        public NormalDistributionPlotter(int width, 
            int height, 
            int offsetXPixel, 
            int offsetYPixel, 
            int radius, 
            int borderX, 
            int borderY, 
            int depthPixel, 
            double standardDerivationOffset, 
            double standardDerivationRadius, 
            double standardDerivationDrillingPoint)
            : base(width, height, offsetXPixel, offsetYPixel, radius, borderX, borderY)
        {
            DepthPixel = depthPixel;
            this.standardDerivationOffset = standardDerivationOffset;
            this.standardDerivationRadius = standardDerivationRadius;
            this.standardDerivationDrillingPoint = standardDerivationDrillingPoint;
        }


        /// <summary>
        /// Plots the sealing slabs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override IEnumerable<T> PlotSealingSlabs<T>()
        {
            var sealingSlabs = base.PlotSealingSlabs<T>();

            var randomDerivationOffset = new NormalDistribution(0, standardDerivationOffset);
            var randomDerivationRadius = new NormalDistribution(0, standardDerivationRadius);
            var randomDerivationDrillingPoint = new NormalDistribution(0, standardDerivationDrillingPoint);

            foreach (var sealingSlab in sealingSlabs)
            {
                sealingSlab.BaseX = sealingSlab.X;
                sealingSlab.BaseY = sealingSlab.Y;

                sealingSlab.OffsetX = (int)GetRandomDerivationOffsetPixel(randomDerivationOffset);
                sealingSlab.OffsetY = (int)GetRandomDerivationOffsetPixel(randomDerivationOffset);

                sealingSlab.OffsetDrillingPointX = (int)GetRandomDerivationDrillingPointPixel(randomDerivationDrillingPoint);
                sealingSlab.OffsetDrillingPointY = (int)GetRandomDerivationDrillingPointPixel(randomDerivationDrillingPoint);

                sealingSlab.Radius = (int)(sealingSlab.Radius + GetRandomDerivationRadiusPixel(randomDerivationRadius));
            }

            return sealingSlabs;
        }


        /// <summary>
        /// Gets the random derivation offset in pixel.
        /// </summary>
        /// <param name="randomDerivationOffset">The random derivation offset.</param>
        /// <returns></returns>
        private double GetRandomDerivationOffsetPixel(NormalDistribution randomDerivationOffset)
        {
            var randomDerivationPercent = randomDerivationOffset.GetRandomValue();
            var randomDerivationPixel = DepthPixel * randomDerivationPercent / 100;

            return randomDerivationPixel;
        }


        /// <summary>
        /// Gets the random derivation radius in pixel.
        /// </summary>
        /// <param name="randomDerivationRadius">The random derivation radius.</param>
        /// <returns></returns>
        private double GetRandomDerivationRadiusPixel(NormalDistribution randomDerivationRadius)
        {
            var randomDerivationRadiusPixel = randomDerivationRadius.GetRandomValue();

            return randomDerivationRadiusPixel;
        }


        /// <summary>
        /// Gets the random derivation drilling point in pixel.
        /// </summary>
        /// <param name="randomDerivationDrillingPoint">The random derivation drilling point.</param>
        /// <returns></returns>
        private double GetRandomDerivationDrillingPointPixel(NormalDistribution randomDerivationDrillingPoint)
        {
            var randomDerivationDrillingPointPixel = randomDerivationDrillingPoint.GetRandomValue();

            return randomDerivationDrillingPointPixel;
        }
    }
}