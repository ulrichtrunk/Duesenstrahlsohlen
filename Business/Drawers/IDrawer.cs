using Business.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Drawers
{
    public interface IDrawer<T> where T : struct
    {
        void DrawSealingSlab(IGrid<T> grid, int x0, int y0, int radius);
    }
}