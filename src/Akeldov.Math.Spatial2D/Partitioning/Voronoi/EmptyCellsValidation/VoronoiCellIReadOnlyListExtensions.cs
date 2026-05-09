using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    internal static partial class VoronoiCellIReadOnlyListExtensions
    {
        public static IReadOnlyList<VoronoiCell<TTexel>> ThrowIfAnyEmptyCell<TTexel>(this IReadOnlyList<VoronoiCell<TTexel>> cells)
            where TTexel : IHasPosition2D
        {
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Items.Count == 0)
                {
                    throw new InvalidOperationException($"Couldn't tessellate by empty cells, empty cell: {cell.Site}.");
                }
            }
            return cells;
        }
    }
}
