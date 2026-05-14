using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS.FlatTop
{
    public static partial class VectorXYIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorQRSInt ToQRSIndex(this VectorXYInt index)
        {
            return new VectorQRSInt(index.X, index.Y - (index.X >= 0 ? index.X / 2 : (index.X - 1) / 2));
        }
    }
}