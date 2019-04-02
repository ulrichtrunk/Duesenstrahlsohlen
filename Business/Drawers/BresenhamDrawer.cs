using Business.Grids;

namespace Business.Drawers
{
    public abstract class BresenhamDrawer<T> : IDrawer<T> where T : struct
    {
        /// <summary>
        /// Draws the sealing slab.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="x0">The x0.</param>
        /// <param name="y0">The y0.</param>
        /// <param name="radius">The radius.</param>
        public virtual void DrawSealingSlab(IGrid<T> grid, int x0, int y0, int radius)
        {
            int x = radius;
            int y = 0;
            int err = 0;
            int prevX = 0;

            while (x >= y)
            {
                if (err <= 0)
                {
                    int y1 = y0 - x;
                    int y2 = y0 - y;
                    int y3 = y0 + y;
                    int y4 = y0 + x;

                    // Top and bottom sections can intersect when
                    // x axis is the one from the previous iteration, or when x equals y (less often)
                    bool topBottomIntersection = prevX == x || x == y;

                    // Middle sections can only intersect on y axis 0
                    bool middleIntersection = y == 0;

                    DrawHorizontalLines(grid, x, x0, y, y1, y2, y3, y4, topBottomIntersection, middleIntersection);

                    prevX = x;
                    y += 1;
                    err += 2 * y + 1;
                }
                else if (err > 0)
                {
                    x -= 1;
                    err -= 2 * x + 1;
                }
            }
        }


        /// <summary>
        /// Checks the bounds.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="y">The y.</param>
        /// <param name="x0">The x0.</param>
        /// <param name="x1">The x1.</param>
        /// <returns></returns>
        protected bool CheckBounds(IGrid<T> grid, int y, ref int x0, ref int x1)
        {
            // Don't draw the whole line if y is outside of the bounds
            if (y < 0 || y >= grid.Height)
            {
                return false;
            }

            if (x0 < 0 || x0 >= grid.Width)
            {
                // If beginning of line is below 0, only draw from pixels from 0
                if (x1 > 0)
                {
                    x0 = 0;
                }
                else
                {
                    return false;
                }
            }

            // If x is greater than the area width, don't draw the following pixels
            if (x1 > grid.Width)
            {
                x1 = grid.Width;
            }

            return true;
        }


        /// <summary>
        /// Gets the istart.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        protected int GetIStart(IGrid<T> grid, int y)
        {
            return y * grid.Width;
        }


        /// <summary>
        /// Gets from.
        /// </summary>
        /// <param name="iStart">The i start.</param>
        /// <param name="x0">The x0.</param>
        /// <returns></returns>
        protected int GetFrom(int iStart, int x0)
        {
            return iStart + x0;
        }


        /// <summary>
        /// Gets to.
        /// </summary>
        /// <param name="iStart">The i start.</param>
        /// <param name="x1">The x1.</param>
        /// <returns></returns>
        protected int GetTo(int iStart, int x1)
        {
            return iStart + x1;
        }


        /// <summary>
        /// Draws the horizontal lines.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="x">The x.</param>
        /// <param name="x0">The x0.</param>
        /// <param name="y">The y.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="y3">The y3.</param>
        /// <param name="y4">The y4.</param>
        /// <param name="topBottomIntersection">if set to <c>true</c> [top bottom intersection].</param>
        /// <param name="middleIntersection">if set to <c>true</c> [middle intersection].</param>
        protected abstract void DrawHorizontalLines(IGrid<T> grid, int x, int x0, int y, int y1, int y2, int y3, int y4, bool topBottomIntersection, bool middleIntersection);
    }
}