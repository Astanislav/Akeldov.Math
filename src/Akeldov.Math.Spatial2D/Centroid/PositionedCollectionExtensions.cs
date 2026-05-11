using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Provides centroid and nearest-item helpers for positioned collections.
    /// </summary>
    public static class PositionedCollectionExtensions
    {
        /// <summary>
        /// Returns the arithmetic center of item positions.
        /// </summary>
        /// <typeparam name="TItem">The positioned item type.</typeparam>
        /// <param name="items">The positioned items.</param>
        /// <returns>The arithmetic center of item positions.</returns>
        public static VectorXY GetBarycenter<TItem>(this IReadOnlyList<TItem> items)
            where TItem : IHasPosition2D
        {
            var sum = VectorXY.Zero;
            for (int k = 0; k < items.Count; k++)
            {
                sum = sum + items[k].Center;
            }
            var res = sum / items.Count;
            return res;
        }

        /// <summary>
        /// Returns the arithmetic center of item positions.
        /// </summary>
        /// <typeparam name="TItem">The positioned item type.</typeparam>
        /// <param name="items">The positioned items.</param>
        /// <returns>The arithmetic center of item positions.</returns>
        public static VectorXY GetBarycenter<TItem>(this TItem[] items)
            where TItem : IHasPosition2D
        {
            var sum = VectorXY.Zero;
            for (int k = 0; k < items.Length; k++)
            {
                sum = sum + items[k].Center;
            }
            var res = sum / items.Length;
            return res;
        }

        /// <summary>
        /// Returns the item closest to the arithmetic center of item positions.
        /// </summary>
        /// <typeparam name="TItem">The positioned item type.</typeparam>
        /// <param name="items">The positioned items.</param>
        /// <returns>The item closest to the arithmetic center.</returns>
        public static TItem GetBarycentric<TItem>(this IReadOnlyList<TItem> items)
            where TItem : IHasPosition2D
        {
            var barycenter = items.GetBarycenter();

            var closestItem = items[0];
            var minDist = closestItem.Center.Distance(barycenter);
            for (int k = 1; k < items.Count; k++)
            {
                var item = items[k];
                var distance = item.Center.Distance(barycenter);

                if (distance < minDist)
                {
                    minDist = distance;
                    closestItem = item;
                }
            }
            return closestItem;
        }

        /// <summary>
        /// Returns the item closest to the arithmetic center of item positions.
        /// </summary>
        /// <typeparam name="TItem">The positioned item type.</typeparam>
        /// <param name="items">The positioned items.</param>
        /// <returns>The item closest to the arithmetic center.</returns>
        public static TItem GetBarycentric<TItem>(this TItem[] items)
            where TItem : IHasPosition2D
        {
            var barycenter = items.GetBarycenter();

            var closestItem = items[0];
            var minDist = closestItem.Center.Distance(barycenter);
            for (int k = 1; k < items.Length; k++)
            {
                var item = items[k];
                var distance = item.Center.Distance(barycenter);

                if (distance < minDist)
                {
                    minDist = distance;
                    closestItem = item;
                }
            }
            return closestItem;
        }

        /// <summary>
        /// Returns the item closest to the specified point.
        /// </summary>
        /// <typeparam name="TItem">The positioned item type.</typeparam>
        /// <param name="items">The positioned items.</param>
        /// <param name="point">The target point.</param>
        /// <returns>The item closest to the target point.</returns>
        public static TItem GetClosestTo<TItem>(this TItem[] items, VectorXY point)
            where TItem : IHasPosition2D
        {
            var closestItem = items[0];
            var minDist = closestItem.Center.Distance(point);
            for (int k = 1; k < items.Length; k++)
            {
                var item = items[k];
                var distance = item.Center.Distance(point);

                if (distance < minDist)
                {
                    minDist = distance;
                    closestItem = item;
                }
            }
            return closestItem;
        }

        /// <summary>
        /// Returns the item closest to the specified point.
        /// </summary>
        /// <typeparam name="TItem">The positioned item type.</typeparam>
        /// <param name="items">The positioned items.</param>
        /// <param name="point">The target point.</param>
        /// <returns>The item closest to the target point.</returns>
        public static TItem GetClosestTo<TItem>(this IReadOnlyList<TItem> items, VectorXY point)
            where TItem : IHasPosition2D
        {
            var closestItem = items[0];
            var minDist = closestItem.Center.Distance(point);
            for (int k = 1; k < items.Count; k++)
            {
                var item = items[k];
                var distance = item.Center.Distance(point);

                if (distance < minDist)
                {
                    minDist = distance;
                    closestItem = item;
                }
            }
            return closestItem;
        }
    }
}
