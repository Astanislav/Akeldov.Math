using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    internal static partial class VoronoiItemPartitionIReadOnlyListExtensions
    {
        public static List<VoronoiItemPartition<TItem>> ExcludeEmptyCells<TItem>(this IReadOnlyList<VoronoiItemPartition<TItem>> cells)
            where TItem : IHasPosition2D
        {
            var nonEmptyCells = new List<VoronoiItemPartition<TItem>>(cells.Count);
            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                if (cell.Items.Count != 0)
                {
                    nonEmptyCells.Add(cell);
                }
            }
            return nonEmptyCells;
        }
    }
}
