using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Provides helper methods for <see cref="Line"/>.
    /// </summary>
    public static class LineExtensions
    {
        /// <summary>
        /// Creates a line perpendicular to the specified line and passing through the specified point.
        /// </summary>
        /// <param name="line">The source line.</param>
        /// <param name="point">The point the perpendicular line must pass through.</param>
        /// <returns>A line perpendicular to <paramref name="line"/> at <paramref name="point"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a perpendicular line cannot be built.</exception>
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

        /// <summary>
        /// Determines whether two points lie on the same side of a line.
        /// </summary>
        /// <param name="line">The line that divides the plane.</param>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <param name="eps">The tolerance used to treat points as lying on the line.</param>
        /// <returns><see langword="true"/> if the points are on the same side or on the line; otherwise, <see langword="false"/>.</returns>
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
