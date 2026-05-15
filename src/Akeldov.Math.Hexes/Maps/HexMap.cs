using Akeldov.Math.Spatial2D;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes
{
    public class HexMap<TValue>
    {
        private VectorXYInt _resolution;
        private TValue[] _values;

        public HexMap(VectorXYInt resolution)
        {
            _resolution = resolution;
            _values = new TValue[resolution.X * resolution.Y];
        }

        public TValue this[VectorXYInt index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index.X < 0 || index.X >= _resolution.X ||
                    index.Y < 0 || index.Y >= _resolution.Y)
                    throw new IndexOutOfRangeException($"Hex index out of bounds: {index}");

                return _values[GetFlatIndex(index)];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index.X < 0 || index.X >= _resolution.X ||
                    index.Y < 0 || index.Y >= _resolution.Y)
                    throw new IndexOutOfRangeException($"Hex index out of bounds: {index}");

                _values[GetFlatIndex(index)] = value;
            }
        }

        public TValue this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _values[index];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _values[index] = value;
        }

        private int GetFlatIndex(VectorXYInt index) => index.Y * _resolution.X + index.X;
    }
}
