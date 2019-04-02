using System.Drawing;

namespace Business.Grids
{
    public class BitGrid : Grid<bool>
    { 
        public BitGrid(int width, int height) 
            : base(width, height)
        {

        }


        /// <summary>
        /// Gets if this intersects with coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override bool Intersects(int x, int y)
        {
            // Intersection information is not available for outside of bounds
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return true;

            // Is there already a part of a circle?
            return Area[y, x] != false;
        }


        /// <summary>
        /// Gets the color of the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override Color GetPixelColor(int x, int y)
        {
            bool value = Area[y, x];

            if (value == true)
            {
                return Color.White;
            }

            return Color.Black;
        }
    }
}