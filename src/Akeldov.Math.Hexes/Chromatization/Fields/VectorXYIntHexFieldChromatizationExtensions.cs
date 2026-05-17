using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Chromatization
{
    public static partial class VectorXYIntExtensions
    {
        public static HexFieldChromatization ToHexFieldChromatization(this VectorXYInt resolution, Layout layout)
        {
            if (resolution.X < 0 || resolution.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution));

            ValidateLayout(layout);

            var count = checked(resolution.X * resolution.Y);
            var chromaticIndices = new byte[count];

            switch (layout)
            {
                case Layout.OddR:
                    FillRowLayoutChromaticIndices(resolution, false, chromaticIndices);
                    break;
                case Layout.EvenR:
                    FillRowLayoutChromaticIndices(resolution, true, chromaticIndices);
                    break;
                case Layout.OddQ:
                    FillColumnLayoutChromaticIndices(resolution, false, chromaticIndices);
                    break;
                case Layout.EvenQ:
                    FillColumnLayoutChromaticIndices(resolution, true, chromaticIndices);
                    break;
            }

            return new HexFieldChromatization(resolution.X, resolution.Y, chromaticIndices);
        }

        private static void FillRowLayoutChromaticIndices(VectorXYInt resolution, bool shiftedRowsUseUpperOffset, byte[] chromaticIndices)
        {
            for (int y = 0; y < resolution.Y; y++)
            {
                int rowStart = y * resolution.X;
                int qOffset = shiftedRowsUseUpperOffset
                    ? (y + (y & 1)) / 2
                    : (y - (y & 1)) / 2;

                for (int x = 0; x < resolution.X; x++)
                {
                    chromaticIndices[rowStart + x] = (byte)PositiveModulo(x - qOffset - y, 3);
                }
            }
        }

        private static void FillColumnLayoutChromaticIndices(VectorXYInt resolution, bool shiftedColumnsUseUpperOffset, byte[] chromaticIndices)
        {
            for (int y = 0; y < resolution.Y; y++)
            {
                int rowStart = y * resolution.X;

                for (int x = 0; x < resolution.X; x++)
                {
                    int rOffset = shiftedColumnsUseUpperOffset
                        ? (x + (x & 1)) / 2
                        : (x - (x & 1)) / 2;

                    chromaticIndices[rowStart + x] = (byte)PositiveModulo(y - rOffset - x, 3);
                }
            }
        }

        private static void ValidateLayout(Layout layout)
        {
            switch (layout)
            {
                case Layout.OddR:
                case Layout.EvenR:
                case Layout.OddQ:
                case Layout.EvenQ:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }
    }
}
