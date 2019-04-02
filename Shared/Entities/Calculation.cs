using NPoco;
using System;

namespace Shared.Entities
{
    [TableName("Calculations")]
    public class Calculation : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int StateId { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal BorderX { get; set; }
        public decimal BorderY { get; set; }
        public decimal DrillingPointDistanceX { get; set; }
        public decimal DrillingPointDistanceY { get; set; }
        public decimal SealingSlabDiameter { get; set; }
        public decimal Depth { get; set; }
        public int PixelsPerMeter { get; set; }
        public decimal StandardDerivationOffset { get; set; }
        public decimal StandardDerivationRadius { get; set; }
        public decimal StandardDerivationDrillingPoint { get; set; }
        public int Iterations { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal? UnsetAreaResult { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }

        // Residual Water
        public decimal WaterLevelDifference { get; set; }
        public decimal SealingSlabThickness { get; set; }
        public decimal PermeabilityOfSoleWithoutUnsetArea { get; set; }
        public decimal PermeabilityOfSoleAtUnsetArea { get; set; }
        public decimal? ResidualWaterResult { get; set; }

        [Ignore]
        public string Time
        {
            get
            {
                if(StartDate.HasValue)
                {
                    if(EndDate.HasValue)
                    {
                        return (EndDate.Value - StartDate.Value).ToShortString();
                    }
                    else
                    {
                        return (DateTime.Now - StartDate.Value).ToShortString();
                    }
                }

                return null;
            }
        }

        [Ignore]
        public int WidthPixels { get { return (int)(Width * PixelsPerMeter); } }
        [Ignore]
        public int HeightPixels { get { return (int)(Height * PixelsPerMeter); } }
        [Ignore]
        public int BorderXPixels { get { return (int)(BorderX * PixelsPerMeter); } }
        [Ignore]
        public int BorderYPixels { get { return (int)(BorderY * PixelsPerMeter); } }
        [Ignore]
        public int DrillingPointDistanceXPixels { get { return (int)(DrillingPointDistanceX * PixelsPerMeter); } }
        [Ignore]
        public int DrillingPointDistanceYPixels { get { return (int)(DrillingPointDistanceY * PixelsPerMeter); } }
        [Ignore]
        public int SealingSlabDiameterPixels { get { return (int)(SealingSlabDiameter * PixelsPerMeter); } }
        [Ignore]
        public int SealingSlabRadiusPixels { get { return (int)(SealingSlabDiameter / 2 * PixelsPerMeter); } }
        [Ignore]
        public int DepthPixels { get { return (int)(Depth * PixelsPerMeter); } }
    }
}
