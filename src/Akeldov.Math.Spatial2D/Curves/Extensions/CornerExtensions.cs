using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Provides helpers for constructing curves from corners.
    /// </summary>
    public static class CornerExtensions
    {
        /// <summary>
        /// Creates an arc tangent to both rays of the angle ABC.
        /// </summary>
        /// <param name="A">A point on the first side of the angle.</param>
        /// <param name="B">The angle vertex.</param>
        /// <param name="C">A point on the second side of the angle.</param>
        /// <param name="radius">The radius of the tangent arc.</param>
        /// <returns>An arc tangent to both sides of the angle.</returns>
        /// <exception cref="ArgumentException">Thrown when the angle is degenerate.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is not finite and positive.</exception>
        public static Arc CreateArcInAngle(VectorXY A, VectorXY B, VectorXY C, float radius)
        {
            if (radius <= 0f || float.IsNaN(radius) || float.IsInfinity(radius))
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be finite and positive.");

            var lineBA = new Line(B, A);
            var lineBC = new Line(B, C);

            if (lineBA.Equals(lineBC))
                throw new ArgumentException("The angle must not be degenerate.");

            return CreateArcInAngle(A, B, C, radius, lineBA, lineBC);
        }

        /// <summary>
        /// Creates an arc tangent to both rays of the angle ABC, rejecting angles whose sides are collinear within the specified tolerance.
        /// </summary>
        /// <param name="A">A point on the first side of the angle.</param>
        /// <param name="B">The angle vertex.</param>
        /// <param name="C">A point on the second side of the angle.</param>
        /// <param name="radius">The radius of the tangent arc.</param>
        /// <param name="epsilon">The non-negative tolerance used to reject nearly degenerate angles.</param>
        /// <returns>An arc tangent to both sides of the angle.</returns>
        /// <exception cref="ArgumentException">Thrown when the angle is degenerate within <paramref name="epsilon"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is not finite and positive, or <paramref name="epsilon"/> is negative, NaN, or infinite.</exception>
        public static Arc CreateArcInAngle(VectorXY A, VectorXY B, VectorXY C, float radius, float epsilon)
        {
            if (radius <= 0f || float.IsNaN(radius) || float.IsInfinity(radius))
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be finite and positive.");

            if (epsilon < 0f || float.IsNaN(epsilon) || float.IsInfinity(epsilon))
                throw new ArgumentOutOfRangeException(nameof(epsilon), "Epsilon must be finite and non-negative.");

            var lineBA = new Line(B, A);
            var lineBC = new Line(B, C);

            if (lineBA.Equals(lineBC) || lineBA.Distance(C) <= epsilon)
                throw new ArgumentException("The angle must not be degenerate.");

            return CreateArcInAngle(A, B, C, radius, lineBA, lineBC);
        }

        private static Arc CreateArcInAngle(VectorXY A, VectorXY B, VectorXY C, float radius, Line lineBA, Line lineBC)
        {
            VectorXY center = GetIncircleCenter(A, B, C, radius);

            VectorXY tanBA = lineBA.ProjectPoint(center);
            VectorXY tanBC = lineBC.ProjectPoint(center);

            float angleA = MathF.Atan2((tanBA - center).Y, (tanBA - center).X);
            float angleC = MathF.Atan2((tanBC - center).Y, (tanBC - center).X);

            float startAngle = angleA.NormalizeAngleRad();
            float endAngle = angleC.NormalizeAngleRad();

            float sweep = (endAngle - startAngle).NormalizeAngleRad();
            if (sweep > MathF.PI)
            {
                var t = startAngle;
                startAngle = endAngle;
                endAngle = t;
            }

            return new Arc(center, radius, startAngle, endAngle);
        }

        private static VectorXY GetIncircleCenter(VectorXY A, VectorXY B, VectorXY C, float radius)
        {
            VectorXY dirBA = (A - B).Normalize();
            VectorXY dirBC = (C - B).Normalize();
            VectorXY bisector = (dirBA + dirBC).Normalize();

            float angle = VectorXY.Angle(dirBA, dirBC).NormalizeAngleRad();
            if (angle <= 0f)
                throw new ArgumentException("The angle must be greater than zero.");

            float distanceToCenter = radius / MathF.Sin(angle / 2f);
            return B + bisector * distanceToCenter;
        }

        /// <summary>
        /// Creates a circle tangent to both rays of the angle ABC.
        /// </summary>
        /// <param name="A">A point on the first side of the angle.</param>
        /// <param name="B">The angle vertex.</param>
        /// <param name="C">A point on the second side of the angle.</param>
        /// <param name="radius">The circle radius.</param>
        /// <returns>A circle tangent to both sides of the angle.</returns>
        /// <exception cref="ArgumentException">Thrown when the angle is degenerate.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is not finite and positive.</exception>
        public static Circle CreateIncircleInAngle(VectorXY A, VectorXY B, VectorXY C, float radius)
        {
            if (radius <= 0f || float.IsNaN(radius) || float.IsInfinity(radius))
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be finite and positive.");

            var lineBA = new Line(B, A);
            var lineBC = new Line(B, C);

            if (lineBA.Equals(lineBC))
                throw new ArgumentException("The angle must not be degenerate.");

            VectorXY center = GetIncircleCenter(A, B, C, radius);

            return new Circle(center, radius);
        }

        /// <summary>
        /// Creates a circle tangent to both rays of the angle ABC, rejecting angles whose sides are collinear within the specified tolerance.
        /// </summary>
        /// <param name="A">A point on the first side of the angle.</param>
        /// <param name="B">The angle vertex.</param>
        /// <param name="C">A point on the second side of the angle.</param>
        /// <param name="radius">The circle radius.</param>
        /// <param name="epsilon">The non-negative tolerance used to reject nearly degenerate angles.</param>
        /// <returns>A circle tangent to both sides of the angle.</returns>
        /// <exception cref="ArgumentException">Thrown when the angle is degenerate within <paramref name="epsilon"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is not finite and positive, or <paramref name="epsilon"/> is negative, NaN, or infinite.</exception>
        public static Circle CreateIncircleInAngle(VectorXY A, VectorXY B, VectorXY C, float radius, float epsilon)
        {
            if (radius <= 0f || float.IsNaN(radius) || float.IsInfinity(radius))
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be finite and positive.");

            if (epsilon < 0f || float.IsNaN(epsilon) || float.IsInfinity(epsilon))
                throw new ArgumentOutOfRangeException(nameof(epsilon), "Epsilon must be finite and non-negative.");

            var lineBA = new Line(B, A);
            var lineBC = new Line(B, C);

            if (lineBA.Equals(lineBC) || lineBA.Distance(C) <= epsilon)
                throw new ArgumentException("The angle must not be degenerate.");

            VectorXY center = GetIncircleCenter(A, B, C, radius);

            return new Circle(center, radius);
        }
    }
}
