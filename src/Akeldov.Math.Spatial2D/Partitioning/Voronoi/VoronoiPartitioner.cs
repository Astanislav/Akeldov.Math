using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    /// <summary>
    /// Partitions positioned items into weighted Voronoi cells.
    /// </summary>
    /// <typeparam name="TItem">The positioned item type to partition.</typeparam>
    public class VoronoiPartitioner<TItem> : IPartitioner<VoronoiCell<TItem>, TItem>
        where TItem : IHasPosition2D
    {
        private readonly Site[] _sites;
        private readonly int _relaxationIterations;
        private readonly EmptyCellPolicy _emptyCellPolicy;

        /// <summary>
        /// Initializes a new Voronoi partitioner with the specified sites.
        /// </summary>
        /// <param name="sites">The Voronoi sites used as cell centers.</param>
        public VoronoiPartitioner(IReadOnlyList<Site> sites)
            : this(sites, 0, EmptyCellPolicy.ThrowException)
        {
        }

        /// <summary>
        /// Initializes a new Voronoi partitioner with an empty-cell policy.
        /// </summary>
        /// <param name="sites">The Voronoi sites used as cell centers.</param>
        /// <param name="emptyCellPolicy">The policy used for cells that receive no items.</param>
        public VoronoiPartitioner(IReadOnlyList<Site> sites, EmptyCellPolicy emptyCellPolicy)
            : this(sites, 0, emptyCellPolicy)
        {
        }

        /// <summary>
        /// Initializes a new Voronoi partitioner with relaxation and empty-cell handling.
        /// </summary>
        /// <param name="sites">The initial Voronoi sites used as cell centers.</param>
        /// <param name="relaxationIterations">The number of centroid relaxation iterations to apply.</param>
        /// <param name="emptyCellPolicy">The policy used for cells that receive no items.</param>
        public VoronoiPartitioner(IReadOnlyList<Site> sites, int relaxationIterations, EmptyCellPolicy emptyCellPolicy)
        {
            if (sites == null)
                throw new ArgumentNullException(nameof(sites));

            if (sites.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(sites));

            if (relaxationIterations < 0)
                throw new ArgumentOutOfRangeException(nameof(relaxationIterations));

            bool hasNonZeroWeight = false;

            _sites = new Site[sites.Count];
            for (int i = 0; i < sites.Count; i++)
            {
                if (sites[i].Weight < 0f || float.IsNaN(sites[i].Weight))
                    throw new ArgumentOutOfRangeException(nameof(sites), "Site weight must be non-negative and not NaN.");

                if (sites[i].Weight > 0f)
                    hasNonZeroWeight = true;

                _sites[i] = sites[i];
            }

            if (!hasNonZeroWeight)
                throw new ArgumentException("At least one site weight must be positive.", nameof(sites));

            _relaxationIterations = relaxationIterations;
            _emptyCellPolicy = emptyCellPolicy;
        }

        /// <summary>
        /// Partitions the specified items into Voronoi cells.
        /// </summary>
        /// <param name="items">The positioned items to partition.</param>
        /// <returns>The generated Voronoi cells.</returns>
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
                if (item is null)
                    throw new ArgumentException("Partition items cannot contain null elements.", nameof(items));

                int siteIndex = sites.GetNearestWeightedSiteIndex(item.Position);
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
