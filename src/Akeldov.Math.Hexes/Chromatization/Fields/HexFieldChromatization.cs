using Akeldov.Math.Hexes.Vectors.QRS;
using System;

namespace Akeldov.Math.Hexes.Chromatization
{
    public sealed class HexFieldChromatization
    {
        public HexFieldChromatization(int width, int height, Layout layout)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            var count = checked(width * height);

            Width = width;
            Height = height;
            Layout = layout;
            ChromaticIndices = new byte[count];

            switch (layout)
            {
                case Layout.OddR:
                    FillRowLayoutChromaticIndices(false);
                    break;
                case Layout.EvenR:
                    FillRowLayoutChromaticIndices(true);
                    break;
                case Layout.OddQ:
                    FillColumnLayoutChromaticIndices(false);
                    break;
                case Layout.EvenQ:
                    FillColumnLayoutChromaticIndices(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }

        public int Width { get; }

        public int Height { get; }

        public Layout Layout { get; }

        public byte[] ChromaticIndices { get; }

        private void FillRowLayoutChromaticIndices(bool shiftedRowsUseUpperOffset)
        {
            for (int y = 0; y < Height; y++)
            {
                int rowStart = y * Width;
                int qOffset = shiftedRowsUseUpperOffset
                    ? (y + (y & 1)) / 2
                    : (y - (y & 1)) / 2;

                for (int x = 0; x < Width; x++)
                {
                    ChromaticIndices[rowStart + x] = (byte)PositiveModulo(x - qOffset - y, 3);
                }
            }
        }

        private void FillColumnLayoutChromaticIndices(bool shiftedColumnsUseUpperOffset)
        {
            for (int y = 0; y < Height; y++)
            {
                int rowStart = y * Width;

                for (int x = 0; x < Width; x++)
                {
                    int rOffset = shiftedColumnsUseUpperOffset
                        ? (x + (x & 1)) / 2
                        : (x - (x & 1)) / 2;

                    ChromaticIndices[rowStart + x] = (byte)PositiveModulo(y - rOffset - x, 3);
                }
            }
        }

        private static int PositiveModulo(int value, int divisor)
        {
            int result = value % divisor;
            return result < 0 ? result + divisor : result;
        }
    }
}
