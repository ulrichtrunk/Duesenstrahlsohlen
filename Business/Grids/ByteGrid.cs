using System.Drawing;

namespace Business.Grids
{
    public class ByteGrid : Grid<byte>
    { 
        public ByteGrid(int width, int height) 
            : base(width, height)
        {

        }

        public override bool Intersects(int x, int y)
        {
            // Intersection information is not available for outside of bounds
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return true;

            // Is there already a part of a circle?
            return Area[y, x] != 0;
        }

        public override Color GetPixelColor(int x, int y)
        {
            byte value = Area[y, x];

            if (value == byte.MaxValue)
                return Color.Red;

            int tone = 255 / 8 * value;

            return Color.FromArgb(tone, tone, tone);
        }
    }
}
