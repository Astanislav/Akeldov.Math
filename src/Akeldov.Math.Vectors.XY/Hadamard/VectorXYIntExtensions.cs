using System.Runtime.CompilerServices;

namespace Akeldov.Math.Vectors.XY
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt HadamardMultiply(this VectorXYInt a, VectorXYInt b)
        {
            return new VectorXYInt(a.X * b.X, a.Y * b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY HadamardMultiply(this VectorXYInt a, VectorXY b)
        {
            return new VectorXY(a.X * b.X, a.Y * b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY HadamardDivide(this VectorXYInt a, VectorXY b)
        {
            return new VectorXY(a.X / b.X, a.Y / b.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXY HadamardDivide(this VectorXYInt a, VectorXYInt b)
        {
            return new VectorXY(a.X / b.X, a.Y / b.Y);
        }
    }
}