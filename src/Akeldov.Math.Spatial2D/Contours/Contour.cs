using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Contours
{
    /// <summary>
    /// Represents a closed two-dimensional contour made from bounded parameterized curves.
    /// </summary>
    public sealed class Contour : IContour
    {
        private readonly IBoundedParameterizedCurve[] _curves;
        private readonly IReadOnlyList<IBoundedParameterizedCurve> _readOnlyCurves;

        /// <summary>
        /// Initializes a new contour from the specified bounded parameterized curves.
        /// </summary>
        /// <param name="curves">The bounded parameterized curves that form the contour.</param>
        public Contour(IReadOnlyList<IBoundedParameterizedCurve> curves)
        {
            if (curves == null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.Count == 0)
                throw new ArgumentException("A contour must contain at least one curve.", nameof(curves));

            _curves = new IBoundedParameterizedCurve[curves.Count];

            for (int i = 0; i < curves.Count; i++)
            {
                _curves[i] = curves[i] ?? throw new ArgumentException("A contour cannot contain null curves.", nameof(curves));
            }

            _readOnlyCurves = Array.AsReadOnly(_curves);
        }

        /// <inheritdoc/>
        public IReadOnlyList<IBoundedParameterizedCurve> Curves => _readOnlyCurves;

        /// <inheritdoc/>
        public bool Contains(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            List<VectorXY> intersections = new List<VectorXY>();
            var ray = new Ray(point);
            int segmentCrossings = 0;

            for (int i = 0; i < _curves.Length; i++)
            {
                var curve = _curves[i];
                if (curve.Distance(point) <= GeometryConstants.GeometryEpsilon)
                    return true;

                if (curve is Segment segment)
                {
                    if (CrossesPositiveXRay(point, segment))
                        segmentCrossings++;

                    continue;
                }

                var newIntersections = curve.GetRayIntersections(ray);

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

        private static bool CrossesPositiveXRay(VectorXY point, Segment segment)
        {
            VectorXY startPoint = segment.StartPoint;
            VectorXY endPoint = segment.EndPoint;

            bool straddlesRay = (startPoint.Y > point.Y) != (endPoint.Y > point.Y);
            if (!straddlesRay)
                return false;

            float x = startPoint.X + (point.Y - startPoint.Y) * (endPoint.X - startPoint.X) / (endPoint.Y - startPoint.Y);
            return x > point.X + GeometryConstants.GeometryEpsilon;
        }

        private static bool IsTangentIntersection(IBoundedParameterizedCurve curve, Ray ray)
        {
            if (curve is Arc arc)
                return IsTangentToCircle(ray, arc.Center, arc.Radius);

            return false;
        }

        private static bool IsTangentToCircle(Ray ray, VectorXY center, float radius)
        {
            VectorXY originToCenter = center - ray.Origin;
            float signedDistance = VectorXY.Cross(ray.Direction, originToCenter);
            return MathF.Abs(MathF.Abs(signedDistance) - radius) <= GeometryConstants.GeometryEpsilon;
        }
    }
}
