using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Grids
{
    public interface IGrid<T> where T : struct
    {
        T[,] Area { get; set; }
        void Clear();
        bool Intersects(int x, int y);
        Color GetPixelColor(int x, int y);
        Bitmap GetBitmap();
        int SetPixelCount { get; set; }
        int TotalPixelCount { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }
}
