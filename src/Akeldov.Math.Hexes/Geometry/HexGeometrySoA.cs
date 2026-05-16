using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Geometry
{
    public sealed class HexGeometrySoA
    {
        public HexGeometrySoA(VectorXY[] centers)
        {
            Centers = centers;
        }

        public VectorXY[] Centers { get; }
    }
}
