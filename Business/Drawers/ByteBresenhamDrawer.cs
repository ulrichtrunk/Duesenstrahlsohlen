using System;
using Business.Grids;

namespace Business.Drawers
{
    public class ByteBresenhamDrawer : BresenhamDrawer<byte>
    {
        private const byte maxColors = 8;
        private const byte originColor = byte.MaxValue;
        private const byte freeColor = byte.MinValue;

        /// <summary>
        /// Draws the sealing slab.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="x0">The x0.</param>
        /// <param name="y0">The y0.</param>
        /// <param name="radius">The radius.</param>
        public override void DrawSealingSlab(IGrid<byte> grid, int x0, int y0, int radius)
        {
            base.DrawSealingSlab(grid, x0, y0, radius);

            DrawOrigin(grid, x0, y0);
        }


        /// <summary>
        /// Draws the origin.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void DrawOrigin(IGrid<byte> grid, int x, int y)
        {
            grid.Area[y, x] = originColor;
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
        protected override void DrawHorizontalLines(IGrid<byte> grid, int x, int x0, int y, int y1, int y2, int y3, int y4, bool topBottomIntersection, bool middleIntersection) 
        {
            unsafe
            {
                // Unsafe pointer for performance (Array-Access)
                fixed (byte* ptr = grid.Area)
                {
                    int x0MinY = x0 - y;
                    int x0PlusY = x0 + y;
                    int x0MinX = x0 - x;
                    int x0PlusX = x0 + x;

                    // Prevent intersections
                    if(!topBottomIntersection)
                    {
                        // Top
                        DrawHorizontalLine(grid, x0MinY, x0PlusY, y1, ptr);

                        // Bottom
                        DrawHorizontalLine(grid, x0MinY, x0PlusY, y4, ptr);
                    }

                    if (!middleIntersection)
                    {
                        // Upper Middle
                        DrawHorizontalLine(grid, x0MinX, x0PlusX, y2, ptr);
                    }

                    // Lower Middle
                    DrawHorizontalLine(grid, x0MinX, x0PlusX, y3, ptr);
                }
            }
        }


        /// <summary>
        /// Draws the horizontal line.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="x0">The x0.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y">The y.</param>
        /// <param name="ptr">The PTR.</param>
        private unsafe void DrawHorizontalLine(IGrid<byte> grid, int x0, int x1, int y, byte* ptr)
        {
            if (!CheckBounds(grid, y, ref x0, ref x1))
            {
                return;
            }

            // Calculate line start and end
            int iStart = GetIStart(grid, y);
            int from = GetFrom(iStart, x0);
            int to = GetTo(iStart, x1);

            for (int x = from; x < to; x++)
            {
                if (ptr[x] == freeColor)
                {
                    grid.SetPixelCount++;
                }

                if (ptr[x] < maxColors)
                {
                    // Increase value when circles intersect
                    ptr[x] = (byte)(ptr[x] + 1);
                }
            }
        }
    }
}