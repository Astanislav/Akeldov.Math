using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using System;

namespace Akeldov.Math.Hexes.Geometry
{
    public class PolyhexGeometry : IPolyhexGeometry
    {
        private Mask _mask;
        private VectorQRSInt _dimension;
        private float _hexApothem;
        private float _hexRadius;

        public PolyhexGeometry(Mask mask, float apothem)
        {
            if (mask.QSize <= 0 || mask.RSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(mask), mask, $"The mask dimensions should be greater than zero");

            _mask = mask;
            _dimension = new VectorQRSInt(_mask.QSize, _mask.RSize);

            _hexApothem = apothem;
            _hexRadius = _hexApothem.ConvertHexApothemToRadius();
        }

        public PolyhexGeometry(VectorQRSInt dimension, float apothem)
        {
            if (dimension.Q <= 0 || dimension.R <= 0)
                throw new ArgumentOutOfRangeException(nameof(dimension), dimension, $"The dimension ({dimension}) Q and R commponents should be greater than zero");

            _dimension = dimension;
            _mask = new bool[_dimension.Q, _dimension.R];

            _hexApothem = apothem;
            _hexRadius = _hexApothem.ConvertHexApothemToRadius();
        }

        public VectorQRSInt Dimension => _dimension;

        public bool this[VectorQRSInt index]
        {
            get => _mask[index.Q, index.R];
        }

        public bool this[int QIndex, int RIndex]
        {
            get => _mask[QIndex, RIndex];
        }

        public Mask Mask => _mask;

        public float HexApothem => _hexApothem;

        public float HexRadius => _hexRadius;

        public int PositiveSize
        {
            get
            {
                var res = 0;

                for (int i = 0; i < _mask.QSize; i++)
                {
                    for (int j = 0; j < _mask.RSize; j++)
                    {
                        if (_mask[i, j])
                            res++;
                    }
                }

                return res;
            }
        }
    }
}
