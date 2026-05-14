using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS.FlatTop
{
    public static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorQRS ToQRS(this VectorXY point, VectorXY newOrigin)
        {
            var shiftedPoint = point - newOrigin;

            var q = 0.6666666666f * shiftedPoint.X;
            var r = 0.5773502588f * shiftedPoint.Y - 0.3333333333f * shiftedPoint.X;

            var res = new VectorQRS(q, r);

            return res;
        }
    }
}