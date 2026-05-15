using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System.Collections.Generic;

namespace Akeldov.Math.Hexes.Geometry.Contours
{
    public static partial class HexMatrixExtensions
    {
        public static Segment[] ToContour(this bool[,] mask, float hexApothem)
        {
            return mask.ToContour(hexApothem, Layout.OddR);
        }

        public static Segment[] ToContour(this bool[,] mask, float hexApothem, Layout layout)
        {
            var hexRadius = 1.1547f * hexApothem;

            int qsize = mask.GetLength(0);
            int rsize = mask.GetLength(1);

            var borderLines = new List<Segment>();

            for (int q = 0; q < qsize; q++)
            {
                for (int r = 0; r < rsize; r++)
                {
                    if (!mask[q, r])
                        continue;

                    VectorXY[] points = Akeldov.Math.Hexes.Geometry.VectorXYExtensions.GetHexVertexes(q, r, hexApothem, hexRadius, layout);

                    var qminClause = q < 1;
                    var rminClause = r < 1;
                    var qmaxClause = q >= qsize - 1;
                    var rmaxClause = r >= rsize - 1;

                    var leftIsBorder = qminClause || !mask[q - 1, r];
                    var rightIsBorder = qmaxClause || !mask[q + 1, r];
                    var topLeftIsBorder = qminClause || rmaxClause || !mask[q - 1, r + 1];
                    var topRightIsBorder = rmaxClause || !mask[q, r + 1];
                    var bottomLeftIsBorder = rminClause || !mask[q, r - 1];
                    var bottomRightIsBorder = qmaxClause || rminClause || !mask[q + 1, r - 1];

                    if (layout == Layout.OddR || layout == Layout.EvenR)
                    {
                        if (leftIsBorder) borderLines.Add(new Segment(points[2], points[3], true, false));
                        if (rightIsBorder) borderLines.Add(new Segment(points[5], points[0], true, false));
                        if (topLeftIsBorder) borderLines.Add(new Segment(points[1], points[2], true, false));
                        if (topRightIsBorder) borderLines.Add(new Segment(points[0], points[1], true, false));
                        if (bottomLeftIsBorder) borderLines.Add(new Segment(points[3], points[4], true, false));
                        if (bottomRightIsBorder) borderLines.Add(new Segment(points[4], points[5], true, false));
                    }
                    else
                    {
                        if (leftIsBorder) borderLines.Add(new Segment(points[3], points[4], true, false));
                        if (rightIsBorder) borderLines.Add(new Segment(points[0], points[1], true, false));
                        if (topLeftIsBorder) borderLines.Add(new Segment(points[2], points[3], true, false));
                        if (topRightIsBorder) borderLines.Add(new Segment(points[1], points[2], true, false));
                        if (bottomLeftIsBorder) borderLines.Add(new Segment(points[4], points[5], true, false));
                        if (bottomRightIsBorder) borderLines.Add(new Segment(points[5], points[0], true, false));
                    }
                }
            }

            return borderLines.ToArray().OrderContour();
        }
    }
}
