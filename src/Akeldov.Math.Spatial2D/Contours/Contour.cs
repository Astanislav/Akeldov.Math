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

            ValidateCurvesFormClosedChain(_curves, nameof(curves));

            _readOnlyCurves = Array.AsReadOnly(_curves);
        }

        /// <inheritdoc/>
        public IReadOnlyList<IBoundedParameterizedCurve> Curves => _readOnlyCurves;

        /// <inheritdoc/>
        public bool Encloses(
            VectorXY point,
            float geometryEpsilon = GeometryConstants.GeometryEpsilon)
        {
            GeometryConstants.ValidateGeometryEpsilon(geometryEpsilon, nameof(geometryEpsilon));

            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            List<VectorXY> intersections = new List<VectorXY>();
            var ray = new Ray(point);
            int segmentCrossings = 0;

            for (int i = 0; i < _curves.Length; i++)
            {
                var curve = _curves[i];
                if (curve.Distance(point) <= geometryEpsilon)
                    return true;

                if (curve is Segment segment)
                {
                    if (CrossesPositiveXRay(point, segment, geometryEpsilon))
                        segmentCrossings++;

                    continue;
                }

                var newIntersections = curve.GetRayIntersections(ray, geometryEpsilon);

                if (newIntersections == null)
                    continue;

                for (int j = 0; j < newIntersections.Count; j++)
                {
                    var intersection = newIntersections[j];
                    if (intersection.X <= point.X + geometryEpsilon)
                        continue;

                    if (IsTangentIntersection(curve, ray, geometryEpsilon))
                        continue;

                    intersections.AddDistinct(intersection, geometryEpsilon);
                }
            }

            return (intersections.Count + segmentCrossings) % 2 == 1;
        }

        private static bool CrossesPositiveXRay(VectorXY point, Segment segment, float geometryEpsilon)
        {
            VectorXY startPoint = segment.StartPoint;
            VectorXY endPoint = segment.EndPoint;

            bool straddlesRay = (startPoint.Y > point.Y) != (endPoint.Y > point.Y);
            if (!straddlesRay)
                return false;

            float x = startPoint.X + (point.Y - startPoint.Y) * (endPoint.X - startPoint.X) / (endPoint.Y - startPoint.Y);
            return x > point.X + geometryEpsilon;
        }

        private static bool IsTangentIntersection(IBoundedParameterizedCurve curve, Ray ray, float geometryEpsilon)
        {
            if (curve is Arc arc)
                return IsTangentToCircle(ray, arc.Center, arc.Radius, geometryEpsilon);

            return false;
        }

        private static bool IsTangentToCircle(Ray ray, VectorXY center, float radius, float geometryEpsilon)
        {
            VectorXY originToCenter = center - ray.Origin;
            float signedDistance = VectorXY.Cross(ray.Direction, originToCenter);
            return MathF.Abs(MathF.Abs(signedDistance) - radius) <= geometryEpsilon;
        }

        private static void ValidateCurvesFormClosedChain(IReadOnlyList<IBoundedParameterizedCurve> curves, string parameterName)
        {
            for (int i = 0; i < curves.Count; i++)
            {
                IBoundedParameterizedCurve currentCurve = curves[i];
                IBoundedParameterizedCurve nextCurve = curves[(i + 1) % curves.Count];

                if (!currentCurve.EndPoint.AlmostEquals(nextCurve.StartPoint))
                    throw new ArgumentException("Contour curves must form a closed continuous chain.", parameterName);
            }
        }
    }
}
