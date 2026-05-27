using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Chromatization
{
    public static partial class VectorXYIntExtensions
    {
        public static HexFieldChromatization ToHexFieldChromatization(this VectorXYInt resolution, Layout layout)
        {
            return new HexFieldChromatization(resolution.X, resolution.Y, layout);
        }
    }
}
