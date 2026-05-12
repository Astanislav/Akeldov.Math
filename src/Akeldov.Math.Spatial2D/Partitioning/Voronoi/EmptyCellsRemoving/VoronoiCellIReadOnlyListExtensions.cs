using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    internal static partial class VoronoiCellIReadOnlyListExtensions
    {
        public static List<VoronoiCell<TItem>> ExcludeEmptyCells<TItem>(this IReadOnlyList<VoronoiCell<TItem>> cells)
            where TItem : IHasPosition2D
        {
            var newArray = new List<VoronoiCell<TItem>>(cells.Count);
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Items.Count != 0)
                {
                    newArray.Add(cell);
                }
            }
            return newArray;
        }
    }
}
