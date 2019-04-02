using NPoco;

namespace Shared.Entities
{
    [TableName("SealingSlabs")]
    public class SealingSlab : BaseEntity<int>, ISealingSlab
    {
        public int CalculationId { get; set; }
        public int Iteration { get; set; }
        public int BaseX { get; set; }
        public int BaseY { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int OffsetDrillingPointX { get; set; }
        public int OffsetDrillingPointY { get; set; }
        public int X { get { return BaseX + OffsetX + OffsetDrillingPointX; } }
        public int Y { get { return BaseY + OffsetY + OffsetDrillingPointY; } }
        public int Radius { get; set; }
    }
}