using System.Collections.Generic;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class IEnumerableExtensions
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

        public static VectorXY Sum(this VectorXY[] vectors)
        {
            var res = VectorXY.Zero;

            for (int i = 0; i < vectors.Length; i++)
            {
                res = res + vectors[i];
            }

            return res;
        }
    }
}
