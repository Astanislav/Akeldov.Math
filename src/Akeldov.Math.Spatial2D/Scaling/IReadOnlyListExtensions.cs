using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D
{
    public static class IReadOnlyListExtensions
    {
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
