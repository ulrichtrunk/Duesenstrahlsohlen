using System.Drawing;

namespace Business.Grids
{
    public abstract class Grid<T> : IGrid<T> where T : struct
    { 
        public T[,] Area { get; set; }
        public int SetPixelCount { get; set; }
        public int TotalPixelCount { get; set; }
        public int FreePixelCount { get { return TotalPixelCount - SetPixelCount; } }
        public double FreeToTotalPixelRatio { get { return FreePixelCount / (double)TotalPixelCount; } }
        public double FreeToTotalPixelRatioPercent { get { return FreeToTotalPixelRatio * 100; } }
        public int Width { get; set; }
        public int Height { get; set; }

        public Grid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Clear();
        }

        /// <summary>
        /// Clears the grid.
        /// </summary>
        public void Clear()
        {
            this.Area = new T[Height, Width];
            this.TotalPixelCount = Width * Height;
            this.SetPixelCount = 0;
        }


        /// <summary>
        /// Gets if this intersects with coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public abstract bool Intersects(int x, int y);


        /// <summary>
        /// Gets the color of the pixel.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public abstract Color GetPixelColor(int x, int y);


        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap()
        {
            var bitmap = new Bitmap(Width, Height);

            // Use bitmap locking for fast performance
            LockBitmap lockBitmap = new LockBitmap(bitmap);
            lockBitmap.LockBits();

            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    lockBitmap.SetPixel(x, y, GetPixelColor(x, y));
                }
            }

            lockBitmap.UnlockBits();

            return bitmap;
        }
    }
}