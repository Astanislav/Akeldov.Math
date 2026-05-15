using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Geometry
{
    public partial class HexGeometryField
    {
        private readonly VectorXYInt _resolution;
        private readonly Layout _layout;

        private float _apothem;
        private float _radius;

        private HexGeometry[] _hexes;

        private VectorXY _size;
        private VectorXY _origin;

        public HexGeometryField(VectorXYInt resolution, Layout layout, float apothem)
        {
            _resolution = resolution;
            _layout = layout;

            _apothem = apothem;
            _radius = apothem.ConvertHexApothemToRadius();

            _size = GetSize(apothem);
            _origin = GetOrigin(layout);

            var adjacentIndices = AdjacentIndicesCalculator.Calculate(resolution, layout);
            _hexes = InitializeHexes(resolution, adjacentIndices);
        }

        public HexGeometry this[VectorXYInt index]
        {
            get
            {
                if (index.X < 0 || index.X >= _resolution.X ||
                    index.Y < 0 || index.Y >= _resolution.Y)
                    throw new IndexOutOfRangeException($"Hex index out of bounds: {index}");

                return _hexes[GetFlatIndex(index)];
            }
        }

        private VectorXY GetOrigin(Layout layout)
        {
            VectorXY _origin;
            switch (layout)
            {
                case Layout.OddR:
                    _origin = new VectorXY(_apothem, _radius);
                    break;
                case Layout.EvenR:
                    _origin = new VectorXY(2 * _apothem, _radius);
                    break;
                case Layout.OddQ:
                    _origin = new VectorXY(_radius, _apothem);
                    break;
                case Layout.EvenQ:
                    _origin = new VectorXY(_radius, 2 * _apothem);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
            return _origin;
        }

        private VectorXY GetSize(float size)
        {
            return _resolution.BoundingBox(size, size.ConvertHexApothemToRadius(), _layout);
        }

        private int GetFlatIndex(VectorXYInt index) => index.Y * _resolution.X + index.X;
    }
}
