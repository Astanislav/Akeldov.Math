using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    /// <summary>
    /// Represents a Voronoi cell and the items assigned to it.
    /// </summary>
    /// <typeparam name="TItem">The positioned item type assigned to the cell.</typeparam>
    public class VoronoiCell<TItem> : IPartition<TItem>
        where TItem : IHasPosition2D
    {
        /// <summary>
        /// Initializes a new Voronoi cell.
        /// </summary>
        /// <param name="site">The site that owns this cell.</param>
        /// <param name="items">The items assigned to this cell.</param>
        public VoronoiCell(Site site, IReadOnlyList<TItem> items)
        {
            Site = site;
            Items = CopyItems(items);
        }

        /// <summary>
        /// Gets the site that owns this cell.
        /// </summary>
        public Site Site { get; }

        /// <summary>
        /// Gets the items assigned to this cell.
        /// </summary>
        public IReadOnlyList<TItem> Items { get; }

        private static IReadOnlyList<TItem> CopyItems(IReadOnlyList<TItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var copy = new TItem[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item is null)
                    throw new ArgumentException("Voronoi cell items cannot contain null elements.", nameof(items));

                copy[i] = item;
            }

            return Array.AsReadOnly(copy);
        }
    }
}
