using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    public class VoronoiPartitioner<TItem> : IPartitioner<VoronoiCell<TItem>, TItem>
        where TItem : IHasPosition2D
    {
        private readonly Site[] _sites;
        private readonly int _relaxationIterations;
        private readonly EmptyCellPolicy _emptyCellPolicy;

        public VoronoiPartitioner(IReadOnlyList<Site> sites)
            : this(sites, 0, EmptyCellPolicy.ThrowException)
        {
        }

        public VoronoiPartitioner(IReadOnlyList<Site> sites, EmptyCellPolicy emptyCellPolicy)
            : this(sites, 0, emptyCellPolicy)
        {
        }

        public VoronoiPartitioner(IReadOnlyList<Site> sites, int relaxationIterations, EmptyCellPolicy emptyCellPolicy)
        {
            if (sites == null)
                throw new ArgumentNullException(nameof(sites));

            if (sites.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(sites));

            if (relaxationIterations < 0)
                throw new ArgumentOutOfRangeException(nameof(relaxationIterations));

            _sites = new Site[sites.Count];
            for (int i = 0; i < sites.Count; i++)
            {
                if (sites[i].Power < 0f || float.IsNaN(sites[i].Power))
                    throw new ArgumentOutOfRangeException(nameof(sites), "Site power must be non-negative and not NaN.");

                _sites[i] = sites[i];
            }

            _relaxationIterations = relaxationIterations;
            _emptyCellPolicy = emptyCellPolicy;
        }

        public IReadOnlyList<VoronoiCell<TItem>> Partition(IReadOnlyList<TItem> items)
        {
            IReadOnlyList<VoronoiCell<TItem>> cells = PartitionInternal(_sites, items);

            for (int i = 0; i < _relaxationIterations; i++)
            {
                var centroidSites = cells.ToCentroidSites();
                cells = PartitionInternal(centroidSites, items);
            }

            switch (_emptyCellPolicy)
            {
                case EmptyCellPolicy.Exclude:
                    return cells.ExcludeEmptyCells();
                case EmptyCellPolicy.LeaveAsIs:
                    return cells;
                default:
                    return cells.ThrowIfAnyEmptyCell();
            }
        }

        private static List<VoronoiCell<TItem>> PartitionInternal(Site[] sites, IReadOnlyList<TItem> items)
        {
            if (sites == null)
                throw new ArgumentNullException(nameof(sites));

            if (sites.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(sites));

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (items.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(items));

            var buckets = new List<TItem>[sites.Length];
            for (int i = 0; i < sites.Length; i++) buckets[i] = new List<TItem>();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                int siteIndex = sites.GetIndexOfClosest(item.Center);
                buckets[siteIndex].Add(item);
            }

            var cells = new List<VoronoiCell<TItem>>(sites.Length);
            for (int i = 0; i < sites.Length; i++)
            {
                var site = sites[i];
                var siteItems = buckets[i];
                var cell = new VoronoiCell<TItem>(site, siteItems);
                cells.Add(cell);
            }

            return cells;
        }
    }
}
