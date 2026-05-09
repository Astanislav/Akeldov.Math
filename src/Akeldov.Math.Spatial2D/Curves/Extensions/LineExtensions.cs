using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Spatial2D.Curves
{
    public static class LineExtensions
    {
        public static Line PerpendicularAt(this Line line, VectorXY point)
        {
            var dir = line.B - line.A;
            float dirLen = dir.Length;
            if (dirLen <= GeometryConstants.GeometryEpsilon)
                throw new InvalidOperationException("Cannot build a perpendicular line for a line with equal endpoints.");

            var perp = new VectorXY(-dir.Y, dir.X);

            float perpLen = perp.Length;
            perp = perp * (1.0f / perpLen);

            float halfLen = dirLen * 0.5f;
            perp = perp * halfLen;

            var a = point - perp;
            var b = point + perp;

            return new Line(a, b);
        }

        public static bool IsSameSide(this Line line, VectorXY p1, VectorXY p2, float eps = GeometryConstants.GeometryEpsilon)
        {
            float s1 = Side(line, p1);
            float s2 = Side(line, p2);

            if (s1.IsAlmostZero(eps) && s2.IsAlmostZero(eps))
                return true;

            if (s1.IsAlmostZero(eps) || s2.IsAlmostZero(eps))
                return true;

            return (s1 > 0 && s2 > 0) || (s1 < 0 && s2 < 0);
        }

        private static float Side(Line line, VectorXY p)
        {
            var ab = line.B - line.A;
            var ap = p - line.A;
            return ab.X * ap.Y - ab.Y * ap.X;
        }
    }
}
