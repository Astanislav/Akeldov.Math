using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D
{
    /// <summary>
    /// Provides scaling helpers for read-only lists.
    /// </summary>
    public static class IReadOnlyListExtensions
    {
        /// <summary>
        /// Scales every item in the list.
        /// </summary>
        /// <typeparam name="T">The scalable item type.</typeparam>
        /// <param name="items">The items to scale.</param>
        /// <param name="scale">The scale factor.</param>
        /// <returns>The scaled items.</returns>
        public static List<T> Scale<T>(this IReadOnlyList<T> items, VectorXY scale)
            where T : IScalable<T>
        {
            var res = new List<T>();

            for (int i = 0; i < items.Count; i++)
            {
                var scaled = items[i].Scale(scale);

                res.Add(scaled);
            }

            return res;
        }
    }
}
