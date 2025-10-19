using System.Collections.Generic;

namespace Akeldov.Math.Vectors.XY
{
    public static class IEnumerableExtensions
    {
        public static VectorXY Sum(this IEnumerable<VectorXY> vectors)
        {
            var res = VectorXY.Zero;

            foreach (var vector in vectors)
            {
                res = res + vector;
            }

            return res;
        }

        public static VectorXY Average(this IEnumerable<VectorXY> vectors)
        {
            var res = VectorXY.Zero;
            var count = 0;

            foreach (var vector in vectors)
            {
                res = res + vector;
                count++;
            }

            return res / count;
        }
    }
}
