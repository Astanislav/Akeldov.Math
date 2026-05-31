using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Geometry
{
    public static class HexFieldTopologyGeometryExtensions
    {
        public static HexCenterMap ToHexFieldGeometry(
            this HexFieldTopologySoA topology,
            VectorXY origin,
            float apothem)
        {
            if (topology == null)
                throw new ArgumentNullException(nameof(topology));

            return new HexCenterMap(
                topology.Width,
                topology.Height,
                origin,
                apothem,
                topology.Layout);
        }
    }
}
