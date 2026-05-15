using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Topology
{
    internal static class AdjacentIndicesCalculator
    {
        private static readonly VectorXYInt[] RowUnshiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(-1, -1),
            new VectorXYInt(0, -1)
        };

        private static readonly VectorXYInt[] RowShiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(1, 0),
            new VectorXYInt(1, 1),
            new VectorXYInt(0, 1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(0, -1),
            new VectorXYInt(1, -1)
        };

        private static readonly VectorXYInt[] ColumnUnshiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(0, 1),
            new VectorXYInt(1, 0),
            new VectorXYInt(1, -1),
            new VectorXYInt(0, -1),
            new VectorXYInt(-1, -1),
            new VectorXYInt(-1, 0)
        };

        private static readonly VectorXYInt[] ColumnShiftedOffsets = new VectorXYInt[]
        {
            new VectorXYInt(0, 1),
            new VectorXYInt(1, 1),
            new VectorXYInt(1, 0),
            new VectorXYInt(0, -1),
            new VectorXYInt(-1, 0),
            new VectorXYInt(-1, 1)
        };

        internal static AdjacentIndices[] Calculate(VectorXYInt resolution, Layout layout)
        {
            switch (layout)
            {
                case Layout.OddR:
                    return CalculateOddR(resolution);
                case Layout.EvenR:
                    return CalculateEvenR(resolution);
                case Layout.OddQ:
                    return CalculateOddQ(resolution);
                case Layout.EvenQ:
                    return CalculateEvenQ(resolution);
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }

        private static AdjacentIndices[] CalculateOddR(VectorXYInt resolution)
        {
            var result = new AdjacentIndices[resolution.X * resolution.Y];

            for (int y = 0; y < resolution.Y; y++)
            {
                var offsets = (y & 1) == 1 ? RowShiftedOffsets : RowUnshiftedOffsets;
                var rowStart = y * resolution.X;
                for (int x = 0; x < resolution.X; x++)
                {
                    var index = new VectorXYInt(x, y);

                    var candidate0 = index + offsets[0];
                    var candidate1 = index + offsets[1];
                    var candidate2 = index + offsets[2];
                    var candidate3 = index + offsets[3];
                    var candidate4 = index + offsets[4];
                    var candidate5 = index + offsets[5];

                    var adjacent0 = candidate0.X < 0 || candidate0.X >= resolution.X || candidate0.Y < 0 || candidate0.Y >= resolution.Y ? index : candidate0;
                    var adjacent1 = candidate1.X < 0 || candidate1.X >= resolution.X || candidate1.Y < 0 || candidate1.Y >= resolution.Y ? index : candidate1;
                    var adjacent2 = candidate2.X < 0 || candidate2.X >= resolution.X || candidate2.Y < 0 || candidate2.Y >= resolution.Y ? index : candidate2;
                    var adjacent3 = candidate3.X < 0 || candidate3.X >= resolution.X || candidate3.Y < 0 || candidate3.Y >= resolution.Y ? index : candidate3;
                    var adjacent4 = candidate4.X < 0 || candidate4.X >= resolution.X || candidate4.Y < 0 || candidate4.Y >= resolution.Y ? index : candidate4;
                    var adjacent5 = candidate5.X < 0 || candidate5.X >= resolution.X || candidate5.Y < 0 || candidate5.Y >= resolution.Y ? index : candidate5;

                    result[rowStart + x] = new AdjacentIndices(adjacent0, adjacent1, adjacent2, adjacent3, adjacent4, adjacent5);
                }
            }

            return result;
        }

        private static AdjacentIndices[] CalculateEvenR(VectorXYInt resolution)
        {
            var result = new AdjacentIndices[resolution.X * resolution.Y];

            for (int y = 0; y < resolution.Y; y++)
            {
                var offsets = (y & 1) == 0 ? RowShiftedOffsets : RowUnshiftedOffsets;
                var rowStart = y * resolution.X;
                for (int x = 0; x < resolution.X; x++)
                {
                    var index = new VectorXYInt(x, y);

                    var candidate0 = index + offsets[0];
                    var candidate1 = index + offsets[1];
                    var candidate2 = index + offsets[2];
                    var candidate3 = index + offsets[3];
                    var candidate4 = index + offsets[4];
                    var candidate5 = index + offsets[5];

                    var adjacent0 = candidate0.X < 0 || candidate0.X >= resolution.X || candidate0.Y < 0 || candidate0.Y >= resolution.Y ? index : candidate0;
                    var adjacent1 = candidate1.X < 0 || candidate1.X >= resolution.X || candidate1.Y < 0 || candidate1.Y >= resolution.Y ? index : candidate1;
                    var adjacent2 = candidate2.X < 0 || candidate2.X >= resolution.X || candidate2.Y < 0 || candidate2.Y >= resolution.Y ? index : candidate2;
                    var adjacent3 = candidate3.X < 0 || candidate3.X >= resolution.X || candidate3.Y < 0 || candidate3.Y >= resolution.Y ? index : candidate3;
                    var adjacent4 = candidate4.X < 0 || candidate4.X >= resolution.X || candidate4.Y < 0 || candidate4.Y >= resolution.Y ? index : candidate4;
                    var adjacent5 = candidate5.X < 0 || candidate5.X >= resolution.X || candidate5.Y < 0 || candidate5.Y >= resolution.Y ? index : candidate5;

                    result[rowStart + x] = new AdjacentIndices(adjacent0, adjacent1, adjacent2, adjacent3, adjacent4, adjacent5);
                }
            }

            return result;
        }

        private static AdjacentIndices[] CalculateOddQ(VectorXYInt resolution)
        {
            var result = new AdjacentIndices[resolution.X * resolution.Y];

            for (int y = 0; y < resolution.Y; y++)
            {
                var rowStart = y * resolution.X;
                for (int x = 0; x < resolution.X; x++)
                {
                    var offsets = (x & 1) == 1 ? ColumnShiftedOffsets : ColumnUnshiftedOffsets;
                    var index = new VectorXYInt(x, y);

                    var candidate0 = index + offsets[0];
                    var candidate1 = index + offsets[1];
                    var candidate2 = index + offsets[2];
                    var candidate3 = index + offsets[3];
                    var candidate4 = index + offsets[4];
                    var candidate5 = index + offsets[5];

                    var adjacent0 = candidate0.X < 0 || candidate0.X >= resolution.X || candidate0.Y < 0 || candidate0.Y >= resolution.Y ? index : candidate0;
                    var adjacent1 = candidate1.X < 0 || candidate1.X >= resolution.X || candidate1.Y < 0 || candidate1.Y >= resolution.Y ? index : candidate1;
                    var adjacent2 = candidate2.X < 0 || candidate2.X >= resolution.X || candidate2.Y < 0 || candidate2.Y >= resolution.Y ? index : candidate2;
                    var adjacent3 = candidate3.X < 0 || candidate3.X >= resolution.X || candidate3.Y < 0 || candidate3.Y >= resolution.Y ? index : candidate3;
                    var adjacent4 = candidate4.X < 0 || candidate4.X >= resolution.X || candidate4.Y < 0 || candidate4.Y >= resolution.Y ? index : candidate4;
                    var adjacent5 = candidate5.X < 0 || candidate5.X >= resolution.X || candidate5.Y < 0 || candidate5.Y >= resolution.Y ? index : candidate5;

                    result[rowStart + x] = new AdjacentIndices(adjacent0, adjacent1, adjacent2, adjacent3, adjacent4, adjacent5);
                }
            }

            return result;
        }

        private static AdjacentIndices[] CalculateEvenQ(VectorXYInt resolution)
        {
            var result = new AdjacentIndices[resolution.X * resolution.Y];

            for (int y = 0; y < resolution.Y; y++)
            {
                var rowStart = y * resolution.X;
                for (int x = 0; x < resolution.X; x++)
                {
                    var offsets = (x & 1) == 0 ? ColumnShiftedOffsets : ColumnUnshiftedOffsets;
                    var index = new VectorXYInt(x, y);

                    var candidate0 = index + offsets[0];
                    var candidate1 = index + offsets[1];
                    var candidate2 = index + offsets[2];
                    var candidate3 = index + offsets[3];
                    var candidate4 = index + offsets[4];
                    var candidate5 = index + offsets[5];

                    var adjacent0 = candidate0.X < 0 || candidate0.X >= resolution.X || candidate0.Y < 0 || candidate0.Y >= resolution.Y ? index : candidate0;
                    var adjacent1 = candidate1.X < 0 || candidate1.X >= resolution.X || candidate1.Y < 0 || candidate1.Y >= resolution.Y ? index : candidate1;
                    var adjacent2 = candidate2.X < 0 || candidate2.X >= resolution.X || candidate2.Y < 0 || candidate2.Y >= resolution.Y ? index : candidate2;
                    var adjacent3 = candidate3.X < 0 || candidate3.X >= resolution.X || candidate3.Y < 0 || candidate3.Y >= resolution.Y ? index : candidate3;
                    var adjacent4 = candidate4.X < 0 || candidate4.X >= resolution.X || candidate4.Y < 0 || candidate4.Y >= resolution.Y ? index : candidate4;
                    var adjacent5 = candidate5.X < 0 || candidate5.X >= resolution.X || candidate5.Y < 0 || candidate5.Y >= resolution.Y ? index : candidate5;

                    result[rowStart + x] = new AdjacentIndices(adjacent0, adjacent1, adjacent2, adjacent3, adjacent4, adjacent5);
                }
            }

            return result;
        }
    }
}
