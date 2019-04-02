using Shared;
using Shared.Entities;
using System.Collections.Generic;

namespace Business.Plotters
{
    interface IPlotter
    {
        int GetColumns();
        int GetRows();
        IEnumerable<T> PlotSealingSlabs<T>() where T : ISealingSlab, new();
    }
}