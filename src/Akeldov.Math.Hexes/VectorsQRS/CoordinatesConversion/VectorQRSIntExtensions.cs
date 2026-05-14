using Akeldov.Math.Spatial2D;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public static partial class VectorQRSIntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ToXYIndex(this VectorQRSInt index, Layout layout)
        {
            if (layout.IsPointyTop())
            {
                int x = index.Q + (index.R >= 0 ? index.R / 2 : (index.R - 1) / 2);
                int y = index.R;

                return new VectorXYInt(x, y);
            }
            else
            {
                int x = index.Q;
                int y = index.R + (index.Q >= 0 ? index.Q / 2 : (index.Q - 1) / 2);

                return new VectorXYInt(x, y);
            }
        }
    }
}