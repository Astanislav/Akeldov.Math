using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    internal static partial class VoronoiItemPartitionIReadOnlyListExtensions
    {
        public static Site[] ToCentroidSites<TItem>(this IReadOnlyList<VoronoiItemPartition<TItem>> cells)
            where TItem : IHasPosition2D
        {
            if (cells == null)
                throw new ArgumentNullException(nameof(cells));

            var newSites = new Site[cells.Count];

            for (int i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];
                var site = cell.Site;
                var items = cell.Items;
                if (items.Count == 0) { newSites[i] = site; continue; }
                var centroid = items.GetCentroid();
                newSites[i] = new Site(centroid, site.Weight);
            }

            return newSites;
        }
    }
}
