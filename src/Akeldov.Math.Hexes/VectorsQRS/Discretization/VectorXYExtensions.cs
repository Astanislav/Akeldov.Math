using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ToXYIndex(this VectorXY point, float hexRadius, VectorXY hexFieldOrigin, Layout layout)
        {
            return point
                .ToQRS(hexFieldOrigin, layout)
                .ToNormalizedAxial(hexRadius)
                .ToQRSIndex(layout)
                .ToXYIndex(layout);
        }
    }
}
