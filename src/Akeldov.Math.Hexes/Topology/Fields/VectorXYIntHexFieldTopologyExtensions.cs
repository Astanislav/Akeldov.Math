using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class VectorXYIntExtensions
    {
        public static HexFieldTopology ToHexHieldTopology(this VectorXYInt resolution, Layout layout)
        {
            return new HexFieldTopology(resolution.X, resolution.Y, layout);
        }
    }
}
