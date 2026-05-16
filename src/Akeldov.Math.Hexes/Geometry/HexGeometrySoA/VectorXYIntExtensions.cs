using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Hexes.Geometry
{
    public static partial class VectorXYIntExtensions
    {
        public static HexGeometrySoA ToHexGeometrySoA(this VectorXYInt resolution, Layout layout, float apothem)
        {
            if (resolution.X < 0 || resolution.Y < 0)
                throw new ArgumentOutOfRangeException(nameof(resolution));

            var radius = apothem.ConvertHexApothemToRadius();
            var count = checked(resolution.X * resolution.Y);
            var centers = new VectorXY[count];

            switch (layout)
            {
                case Layout.OddR:
                    FillRowLayoutCenters(resolution, false, apothem, radius, centers);
                    break;
                case Layout.EvenR:
                    FillRowLayoutCenters(resolution, true, apothem, radius, centers);
                    break;
                case Layout.OddQ:
                    FillColumnLayoutCenters(resolution, false, apothem, radius, centers);
                    break;
                case Layout.EvenQ:
                    FillColumnLayoutCenters(resolution, true, apothem, radius, centers);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }

            return new HexGeometrySoA(centers);
        }

        private static void FillRowLayoutCenters(
            VectorXYInt resolution,
            bool evenRowsAreShifted,
            float apothem,
            float radius,
            VectorXY[] centers)
        {
            var origin = evenRowsAreShifted
                ? new VectorXY(2f * apothem, radius)
                : new VectorXY(apothem, radius);

            for (int y = 0; y < resolution.Y; y++)
            {
                var rowStart = y * resolution.X;
                var rowIsShifted = ((y & 1) == 0) == evenRowsAreShifted;
                var xShift = rowIsShifted ? apothem : 0f;
                var centerY = origin.Y + 1.5f * radius * y;

                for (int x = 0; x < resolution.X; x++)
                {
                    centers[rowStart + x] = new VectorXY(
                        origin.X + x * 2f * apothem + xShift,
                        centerY);
                }
            }
        }

        private static void FillColumnLayoutCenters(
            VectorXYInt resolution,
            bool evenColumnsAreShifted,
            float apothem,
            float radius,
            VectorXY[] centers)
        {
            var origin = evenColumnsAreShifted
                ? new VectorXY(radius, 2f * apothem)
                : new VectorXY(radius, apothem);

            for (int y = 0; y < resolution.Y; y++)
            {
                var rowStart = y * resolution.X;
                var baseY = origin.Y + y * 2f * apothem;

                for (int x = 0; x < resolution.X; x++)
                {
                    var columnIsShifted = ((x & 1) == 0) == evenColumnsAreShifted;
                    var yShift = columnIsShifted ? apothem : 0f;

                    centers[rowStart + x] = new VectorXY(
                        origin.X + 1.5f * radius * x,
                        baseY + yShift);
                }
            }
        }
    }
}
