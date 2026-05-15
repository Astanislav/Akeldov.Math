using Akeldov.Math.Spatial2D;
using Akeldov.Math.Hexes.Vectors.QRS;

namespace Akeldov.Math.Hexes.Rectangles
{
    public readonly struct HexOrientedRectangle
    {
        private readonly VectorXY _center;
        private readonly VectorXY _size;
        private readonly SixfoldAngle _rotation;

        private readonly VectorXY _bottomLeft;
        private readonly VectorXY _bottomRight;
        private readonly VectorXY _topLeft;
        private readonly VectorXY _topRight;

        public HexOrientedRectangle(VectorXY center, VectorXY size, SixfoldAngle rotation)
        {
            _center = center;
            _size = size;
            _rotation = rotation;

            var (halfSizeX, halfSizeY) = size / 2;

            _bottomLeft = (_center + new VectorXY(-halfSizeX, -halfSizeY)).Rotate(_center, rotation);
            _bottomRight = (_center + new VectorXY(halfSizeX, -halfSizeY)).Rotate(_center, rotation);
            _topLeft = (_center + new VectorXY(-halfSizeX, halfSizeY)).Rotate(_center, rotation);
            _topRight = (_center + new VectorXY(halfSizeX, halfSizeY)).Rotate(_center, rotation);
        }

        public VectorXY Center => _center;

        public VectorXY Size => _size;

        public SixfoldAngle Rotation => _rotation;

        public VectorXY BottomLeft => _bottomLeft;

        public VectorXY BottomRight => _bottomRight;

        public VectorXY TopLeft => _topLeft;

        public VectorXY TopRight => _topRight;

        public VectorXY GetLocalCoordinates(VectorXY point)
        {
            var localCoordinates = (point - BottomLeft).Rotate(Rotation.Negate());
            return localCoordinates;
        }

        public VectorXY GetLocalNormalizedCoordinates(VectorXY point)
        {
            var localCoordinates = (point - BottomLeft).Rotate(Rotation.Negate());
            return localCoordinates.HadamardDivide(Size);
        }

        public VectorXY GetLocalCoordinates(VectorXY point, bool isClamped)
        {
            var localCoordinates = (point - BottomLeft).Rotate(Rotation.Negate());
            if (isClamped)
                localCoordinates = localCoordinates.Clamp(VectorXY.Zero, Size);
            return localCoordinates;
        }

        public VectorXY GetLocalNormalizedCoordinates(VectorXY point, bool isClamped)
        {
            var localCoordinates = (point - BottomLeft).Rotate(Rotation.Negate());
            var normalizedLocalCoordinates = localCoordinates.HadamardDivide(Size);
            if (isClamped)
                normalizedLocalCoordinates = normalizedLocalCoordinates.Clamp(VectorXY.Zero, VectorXY.One);
            return normalizedLocalCoordinates;
        }

        public static HexOrientedRectangle CreateFromBottomLeftPoint(VectorXY bottomLeftPoint, VectorXY size, SixfoldAngle rotation)
        {
            var center = (bottomLeftPoint + size * 0.5f).Rotate(bottomLeftPoint, rotation);
            return new HexOrientedRectangle(center, size, rotation);
        }
    }
}
