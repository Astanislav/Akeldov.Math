using System.Runtime.CompilerServices;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY HadamardMultiply(this VectorXY a, VectorXY b)
        {
            return new VectorXY(a.X * b.X, a.Y * b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY HadamardMultiply(this VectorXY a, VectorXYInt b)
        {
            return new VectorXY(a.X * b.X, a.Y * b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY HadamardDivide(this VectorXY a, VectorXY b)
        {
            return new VectorXY(a.X / b.X, a.Y / b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY HadamardDivide(this VectorXY a, VectorXYInt b)
        {
            return new VectorXY(a.X / b.X, a.Y / b.Y);
        }
    }
}