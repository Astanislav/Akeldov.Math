using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Geometry
{
    public sealed class HexFieldGeometry
    {
        private readonly int _width;
        private readonly int _height;

        public HexFieldGeometry(int width, int height, VectorXY[] centers)
        {
            _width = width;
            _height = height;
            Centers = centers;
        }

        public int Width => _width;

        public int Height => _height;

        public VectorXY[] Centers { get; }
    }
}
