using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS.PointyTop
{
    public static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ToXYIndex(this VectorXY point, float hexRadius, VectorXY hexFieldOrigin)
        {
            return point
                .ToQRS(hexFieldOrigin)
                .ToNormalizedAxial(hexRadius)
                .ToQRSIndex()
                .ToXYIndex();
        }
    }
}
