using System.Runtime.CompilerServices;
using System;

namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public static partial class VectorQRSExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorQRS ToNormalizedAxial(this VectorQRS fractionalPoint, float hexRadius)
        {
            if (hexRadius == 0)
                throw new ArgumentOutOfRangeException(nameof(hexRadius), hexRadius, $"Couldn't devide on zero {nameof(hexRadius)}");

            return fractionalPoint / hexRadius;
        }
    }
}
