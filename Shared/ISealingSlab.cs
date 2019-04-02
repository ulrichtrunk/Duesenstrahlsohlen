namespace Shared
{
    public interface ISealingSlab
    {
        int BaseX { get; set; }
        int BaseY { get; set; }
        int OffsetX { get; set; }
        int OffsetY { get; set; }
        int OffsetDrillingPointX { get; set; }
        int OffsetDrillingPointY { get; set; }
        int X { get; }
        int Y { get; }
        int Radius { get; set; }
    }
}