using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a closed two-dimensional contour made from curves.
    /// </summary>
    public sealed class Contour : IContour
    {
        private readonly ICurve[] _curves;

        /// <summary>
        /// Initializes a new contour from the specified curves.
        /// </summary>
        /// <param name="curves">The curves that form the contour.</param>
        public Contour(IReadOnlyList<ICurve> curves)
        {
            if (curves == null)
                throw new ArgumentNullException(nameof(curves));

            if (curves.Count == 0)
                throw new ArgumentException("A contour must contain at least one curve.", nameof(curves));

            _curves = new ICurve[curves.Count];

            for (int i = 0; i < curves.Count; i++)
            {
                _curves[i] = curves[i] ?? throw new ArgumentException("A contour cannot contain null curves.", nameof(curves));
            }
        }

        /// <inheritdoc/>
        public IReadOnlyList<ICurve> Curves => _curves;

        /// <inheritdoc/>
        public bool Contains(VectorXY point)
        {
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
