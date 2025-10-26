using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY Rotate(this VectorXYInt point, float angleRad)
        {
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            float x = point.X * cos - point.Y * sin;
            float y = point.X * sin + point.Y * cos;

            return new VectorXY(x, y);
        }
    }
}