using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Geometry
{
    public class BarycentricSoA
    {
        public BarycentricSoA(
            VectorXY[] points,
            Triplet<VectorXYInt>[] indices, 
            Triplet<byte>[] chromaticIndices, 
            Triplet<float>[] barycentricCoordinates)
        {
            Points = points;
            Indices = indices;
            ChromaticIndices = chromaticIndices;
            BarycentricCoordinates = barycentricCoordinates;
        }

        public VectorXY[] Points { get; }

        public Triplet<VectorXYInt>[] Indices { get; }

        public Triplet<byte>[] ChromaticIndices { get; }

        public Triplet<float>[] BarycentricCoordinates { get; }
    }
}
