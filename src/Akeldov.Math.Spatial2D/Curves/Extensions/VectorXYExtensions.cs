using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Provides curve-related helpers for <see cref="VectorXY"/>.
    /// </summary>
    public static class VectorXYExtensions
    {
        /// <summary>
        /// Determines whether a point lies inside a closed contour made from curves.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="curves">The contour curves.</param>
        /// <returns><see langword="true"/> if the point lies inside or on the contour; otherwise, <see langword="false"/>.</returns>
        public static bool IsInsideContour(this VectorXY point, ICurve[] curves)
        {
            List<VectorXY> intersections = new List<VectorXY>();
            var ray = new Ray(point);
            int segmentCrossings = 0;

            for (int i = 0; i < curves.Length; i++)
            {
                var curve = curves[i];
                if (curve.Distance(point) <= GeometryConstants.GeometryEpsilon)
                    return true;

                if (curve is Segment segment)
                {
                    if (CrossesPositiveXRay(point, segment))
                        segmentCrossings++;

                    continue;
                }

                var newIntersections = curve.RayIntersections(ray);

                if (newIntersections == null)
                    continue;

                for (int j = 0; j < newIntersections.Count; j++)
                {
                    var intersection = newIntersections[j];
                    if (intersection.X <= point.X + GeometryConstants.GeometryEpsilon)
                        continue;

                    if (IsTangentIntersection(curve, ray))
                        continue;

                    intersections.AddDistinct(intersection);
                }
            }

            return (intersections.Count + segmentCrossings) % 2 == 1;
        }

        /// <summary>
        /// Determines whether a point lies inside a closed contour made from segments.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="curves">The contour segments.</param>
        /// <returns><see langword="true"/> if the point lies inside or on the contour; otherwise, <see langword="false"/>.</returns>
        public static bool IsInsideContour(this VectorXY point, Segment[] curves)
        {
            int crossings = 0;

            for (int i = 0; i < curves.Length; i++)
            {
                var segment = curves[i];
                if (IsPointOnSegment(point, segment))
                    return true;

                if (CrossesPositiveXRay(point, segment))
                    crossings++;
            }

            return crossings % 2 == 1;
        }

        /// <summary>
        /// Determines whether a point's angular position lies on an arc.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="arc">The arc that supplies the center and angular span.</param>
        /// <returns><see langword="true"/> if the point lies within the arc's angular span; otherwise, <see langword="false"/>.</returns>
        public static bool IsOnTheArc(this VectorXY point, Arc arc)
        {
            VectorXY toPoint = (point - arc.Center).Normalize();
            float angle = MathF.Atan2(toPoint.Y, toPoint.X);
            angle = angle.NormalizeAngleRad();

            float start = arc.StartAngle;
            float stop = arc.EndAngle;

            return arc.IsFullCircle || angle.IsAngleWithinArc(start, stop);
        }

        private static bool CrossesPositiveXRay(VectorXY point, Segment segment)
        {
            VectorXY a = segment.A;
            VectorXY b = segment.B;

            bool straddlesRay = (a.Y > point.Y) != (b.Y > point.Y);
            if (!straddlesRay)
                return false;

            float x = a.X + (point.Y - a.Y) * (b.X - a.X) / (b.Y - a.Y);
            return x > point.X + GeometryConstants.GeometryEpsilon;
        }

        private static bool IsPointOnSegment(VectorXY point, Segment segment)
        {
            VectorXY ab = segment.B - segment.A;
            VectorXY ap = point - segment.A;

            if (!VectorXY.Cross(ab, ap).IsAlmostZero())
                return false;

            float dot = VectorXY.Dot(ap, ab);
            if (dot < -GeometryConstants.GeometryEpsilon)
                return false;

            return dot <= ab.SQRLength + GeometryConstants.GeometryEpsilon;
        }

        private static bool IsTangentIntersection(ICurve curve, Ray ray)
        {
            if (curve is Circle circle)
                return IsTangentToCircle(ray, circle.Center, circle.Radius);

            if (curve is Arc arc)
                return IsTangentToCircle(ray, arc.Center, arc.Radius);

            return false;
        }

        private static bool IsTangentToCircle(Ray ray, VectorXY center, float radius)
        {
            VectorXY originToCenter = center - ray.Origin;
            float signedDistance = VectorXY.Cross(ray.Dir, originToCenter);
            return MathF.Abs(MathF.Abs(signedDistance) - radius) <= GeometryConstants.GeometryEpsilon;
        }
    }
}
