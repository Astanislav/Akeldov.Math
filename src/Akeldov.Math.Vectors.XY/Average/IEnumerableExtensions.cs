using System;
using System.Collections.Generic;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class IEnumerableExtensions
    {
        public static VectorXY Average(this IEnumerable<VectorXY> vectors)
        {
            if (vectors is null)
                throw new ArgumentNullException(nameof(vectors));

            var res = VectorXY.Zero;
            var count = 0;

            foreach (var vector in vectors)
            {
                res = res + vector;
                count++;
            }

            if (count == 0)
                throw new InvalidOperationException("Sequence contains no elements.");

            return res / count;
        }

        public static VectorXY Average(this VectorXY[] vectors)
        {
            if (vectors is null)
                throw new ArgumentNullException(nameof(vectors));

            if (vectors.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements.");

            var res = VectorXY.Zero;

            for (int i = 0; i < vectors.Length; i++)
            {
                res = res + vectors[i];
            }

            return res / vectors.Length;
        }
    }
}
