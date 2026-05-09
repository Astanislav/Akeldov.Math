using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    public class VoronoiCell<TItem> : IPartition<TItem>
        where TItem : IHasPosition2D
    {
        public VoronoiCell(Site site, IReadOnlyList<TItem> items)
        {
            Site = site;
            Items = items;
        }

        public Site Site { get; }

        public IReadOnlyList<TItem> Items { get; }
    }
}
