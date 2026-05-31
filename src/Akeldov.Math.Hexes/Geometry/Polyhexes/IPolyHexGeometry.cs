using Akeldov.Math.Hexes.Topology;

namespace Akeldov.Math.Hexes.Geometry
{
    public interface IPolyhexGeometry : IPolyhex
    {
        float HexApothem { get; }

        float HexRadius { get; }
    }
}
