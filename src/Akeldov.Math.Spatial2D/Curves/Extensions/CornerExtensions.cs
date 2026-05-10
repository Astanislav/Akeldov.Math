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
        /// <exception cref="ArgumentException">Thrown when the angle is not greater than zero.</exception>
        public static Arc CreateArcInAngle(VectorXY A, VectorXY B, VectorXY C, float radius)
        {
            VectorXY dirBA = (A - B).Normalize();
            VectorXY dirBC = (C - B).Normalize();

            float angle = VectorXY.Angle(dirBA, dirBC).NormalizeAngleRad();
            if (angle <= 0f)
                throw new ArgumentException("The angle must be greater than zero.");

            VectorXY bisector = (dirBA + dirBC).Normalize();

            float distanceToCenter = radius / MathF.Sin(angle / 2f);
            VectorXY center = B + bisector * distanceToCenter;

            var lineBA = new Line(B, A);
            var lineBC = new Line(B, C);

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

        /// <summary>
        /// Creates a circle tangent to both rays of the angle ABC.
        /// </summary>
        /// <param name="A">A point on the first side of the angle.</param>
        /// <param name="B">The angle vertex.</param>
        /// <param name="C">A point on the second side of the angle.</param>
        /// <param name="radius">The circle radius.</param>
        /// <returns>A circle tangent to both sides of the angle.</returns>
        /// <exception cref="ArgumentException">Thrown when the angle is not greater than zero.</exception>
        public static Circle CreateIncircleInAngle(VectorXY A, VectorXY B, VectorXY C, float radius)
        {
            VectorXY dirBA = (A - B).Normalize();
            VectorXY dirBC = (C - B).Normalize();

            VectorXY bisector = (dirBA + dirBC).Normalize();

            float angle = VectorXY.Angle(dirBA, dirBC).NormalizeAngleRad();
            if (angle <= 0f)
                throw new ArgumentException("The angle must be greater than zero.");

            float distanceToCenter = radius / MathF.Sin(angle / 2f);

            VectorXY center = B + bisector * distanceToCenter;

            return new Circle(center, radius);
        }
    }
}
