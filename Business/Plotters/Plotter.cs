using Shared;
using Shared.Entities;
using System.Collections.Generic;

namespace Business.Plotters
{
    public class Plotter : IPlotter
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int OffsetXPixel { get; private set; }
        public int OffsetYPixel { get; private set; }
        public int Radius { get; private set; }
        public int Diameter { get; private set; }
        public int BorderX { get; private set; }
        public int BorderY { get; private set; }

        public Plotter(int width, int height, int offsetXPixel, int offsetYPixel, int radius, int borderX = 0, int borderY = 0)
        {
            Width = width;
            Height = height;
            OffsetXPixel = offsetXPixel;
            OffsetYPixel = offsetYPixel;
            Radius = radius;
            BorderX = borderX;
            BorderY = borderY;
            Diameter = Radius * 2;
        }


        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <returns></returns>
        public int GetColumns()
        {
            return Diameter > 0 ? (Width - 2 * BorderX) / Radius * 2 : 0;
        }


        /// <summary>
        /// Gets the rows.
        /// </summary>
        /// <returns></returns>
        public int GetRows()
        {
            return Diameter > 0 ? (Height - 2 * BorderY) / Radius * 2 : 0;
        }


        /// <summary>
        /// Plots the sealing slabs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IEnumerable<T> PlotSealingSlabs<T>() where T : ISealingSlab, new()
        {
            var sealingSlabs = new List<T>();

            int columns = Diameter > 0 ? (Width - 2 * BorderX) / Radius * 2 : 0;
            int rows = Diameter > 0 ? (Height - 2 * BorderY) / Radius * 2 : 0;

            for (int row = 0; row < columns; row++)
            {
                bool alternateRow = row % 2 == 1;

                for (int col = 0; col < rows; col++)
                {
                    var r = Radius;
                    var x = col * OffsetXPixel + (alternateRow ? (OffsetXPixel / 2) : 0);
                    var y = row * OffsetYPixel;

                    if (x <= Width && y <= Height && x >= 0 && y >= 0)
                    {
                        sealingSlabs.Add(new T { BaseX = x + BorderX, BaseY = y + BorderY, Radius = r });
                    }
                }
            }

            return sealingSlabs;
        }
    }
}