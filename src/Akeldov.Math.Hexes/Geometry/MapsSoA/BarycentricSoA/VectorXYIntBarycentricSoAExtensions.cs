using Akeldov.Math.Hexes.Chromatization;
using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Geometry
{
    public static partial class VectorXYIntExtensions
    {
        public static BarycentricSoA GetBarycentricSoA(
            this VectorXYInt resolution,
            VectorXY p0,
            VectorXY p1,
            VectorXY origin,
            float radius)
        {
            return resolution.GetBarycentricSoA(p0, p1, origin, radius, Layout.OddR);
        }

        public static BarycentricSoA GetBarycentricSoA(
            this VectorXYInt resolution,
            VectorXY p0,
            VectorXY p1,
            VectorXY origin,
            float radius,
            Layout layout)
        {
            var apothem = radius.ConvertHexRadiusToApothem();

            var resLength = resolution.X * resolution.Y;
            var points = new VectorXY[resLength];
            var indices = new Triplet<VectorXYInt>[resLength];
            var chromaticIndices = new Triplet<byte>[resLength];
            var barycentricCoordinates = new Triplet<float>[resLength];

            var stepX = (p1.X - p0.X) / (resolution.X - 1);
            var stepY = (p1.Y - p0.Y) / (resolution.Y - 1);

            var baseindex = 0;
            var index = 0;
            for (int r = 0; r < resolution.Y; r++)
            {
                baseindex = r * resolution.X;
                for (int c = 0; c < resolution.X; c++)
                {
                    index = baseindex + c;

                    
                    var point = new VectorXY(c * stepX, r * stepY);
                    points[index] = point;

                    
                    var (mainIndex, hexVertex) = point.GetClosestVertexIndex(apothem, radius, origin, layout);
                    var (leftEdge, rightEdge) = hexVertex.GetAdjacentEdges(layout);
                    var leftIndex = mainIndex.GetAdjacent(leftEdge, layout);
                    var rightIndex = mainIndex.GetAdjacent(rightEdge, layout);
                    indices[index] = new Triplet<VectorXYInt>(mainIndex, leftIndex, rightIndex);

                    
                    var mainChromaticIndex = (byte)mainIndex.GetChromaticClass(layout);
                    var leftChromaticIndex = (byte)leftIndex.GetChromaticClass(layout);
                    var rightChromaticIndex = (byte)rightIndex.GetChromaticClass(layout);
                    var chromaticTriplet = new Triplet<byte>(mainChromaticIndex, leftChromaticIndex, rightChromaticIndex);
                    chromaticIndices[index] = chromaticTriplet;

                    
                    var mainCenter = mainIndex.GetHexCenter(apothem, radius, origin, layout);
                    var leftCenter = leftIndex.GetHexCenter(apothem, radius, origin, layout);
                    var rightCenter = rightIndex.GetHexCenter(apothem, radius, origin, layout);
                    var barycentricCoordinatesTriplet = point.BarycentricCoordinates(mainCenter, leftCenter, rightCenter);
                    barycentricCoordinates[index] = barycentricCoordinatesTriplet;
                }
            }

            return new BarycentricSoA(points, indices, chromaticIndices, barycentricCoordinates);
        }
    }
}
