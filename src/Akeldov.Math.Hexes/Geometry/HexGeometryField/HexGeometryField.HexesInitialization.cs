using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Geometry
{
    public partial class HexGeometryField
    {
        private HexGeometry[] InitializeHexes(VectorXYInt resolution, AdjacentIndices[] adjacentIndices)
        {
            var hexes = new HexGeometry[resolution.X * resolution.Y];
            for (int y = 0; y < resolution.Y; y++)
            {
                for (int x = 0; x < resolution.X; x++)
                {
                    var index = new VectorXYInt(x, y);
                    var flatIndex = GetFlatIndex(index);
                    var center = CalculateHexCenter(index);

                    hexes[flatIndex] = new HexGeometry(index, adjacentIndices[flatIndex], center);
                }
            }
            return hexes;
        }

        private VectorXY CalculateHexCenter(VectorXYInt index)
        {
            return index.GetHexCenter(_apothem, _radius, _origin, _layout);
        }
    }
}
