using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Regions
{
    /// <summary>
    /// Represents a filled two-dimensional region defined by one or more closed contours.
    /// </summary>
    public sealed class Region : IRegion
    {
        private readonly IContour[] _contours;
        private readonly IReadOnlyList<IContour> _readOnlyContours;
        private readonly IBoundedParameterizedCurve[] _boundaryCurves;

        /// <summary>
        /// Initializes a new region from the specified contours and fill rule.
        /// </summary>
        /// <param name="contours">The contours that define the region.</param>
        /// <param name="fillRule">The fill rule used to interpret the contours.</param>
        public Region(IReadOnlyList<IContour> contours, FillRule fillRule = FillRule.EvenOdd)
        {
            if (contours == null)
                throw new ArgumentNullException(nameof(contours));

            if (contours.Count == 0)
                throw new ArgumentException("A region must contain at least one contour.", nameof(contours));

            if (fillRule != FillRule.EvenOdd)
                throw new ArgumentOutOfRangeException(nameof(fillRule), "The fill rule is not supported.");

            _contours = new IContour[contours.Count];
            var contourCurves = new IBoundedParameterizedCurve[contours.Count][];
            var boundaryCurves = new List<IBoundedParameterizedCurve>();

            for (int i = 0; i < contours.Count; i++)
            {
                IContour contour = contours[i] ?? throw new ArgumentException("A region cannot contain null contours.", nameof(contours));
                IReadOnlyList<IBoundedParameterizedCurve> curves = contour.Curves;

                if (curves == null || curves.Count == 0)
                    throw new ArgumentException("Region contours must expose at least one bounded parameterized curve.", nameof(contours));

                var contourCurveArray = new IBoundedParameterizedCurve[curves.Count];
                for (int j = 0; j < curves.Count; j++)
                {
                    IBoundedParameterizedCurve curve = curves[j] ?? throw new ArgumentException("Region contour curves must not contain null values.", nameof(contours));
                    contourCurveArray[j] = curve;
                    boundaryCurves.Add(curve);
                }

                _contours[i] = contour;
                contourCurves[i] = contourCurveArray;
            }

            ValidateContoursDoNotIntersect(contourCurves, nameof(contours));

            FillRule = fillRule;
            _readOnlyContours = Array.AsReadOnly(_contours);
            _boundaryCurves = boundaryCurves.ToArray();
        }

        /// <inheritdoc/>
        public IReadOnlyList<IContour> Contours => _readOnlyContours;

        /// <inheritdoc/>
        public FillRule FillRule { get; }

        /// <inheritdoc/>
        public bool Contains(
            VectorXY point,
            float geometryEpsilon = GeometryConstants.GeometryEpsilon)
        {
            GeometryConstants.ValidateGeometryEpsilon(geometryEpsilon, nameof(geometryEpsilon));

            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            if (IsOnBoundary(point, geometryEpsilon))
                return true;

            int containingContours = 0;

            for (int i = 0; i < _contours.Length; i++)
            {
                if (_contours[i].Encloses(point, geometryEpsilon))
                    containingContours++;
            }

            return containingContours % 2 == 1;
        }

        private bool IsOnBoundary(VectorXY point, float geometryEpsilon)
        {
            for (int i = 0; i < _boundaryCurves.Length; i++)
            {
                if (_boundaryCurves[i].Distance(point) <= geometryEpsilon)
                    return true;
            }

            return false;
        }

        private static void ValidateContoursDoNotIntersect(IBoundedParameterizedCurve[][] contours, string parameterName)
        {
            for (int contourIndex = 0; contourIndex < contours.Length; contourIndex++)
            {
                IBoundedParameterizedCurve[] contour = contours[contourIndex];

                for (int otherContourIndex = contourIndex + 1; otherContourIndex < contours.Length; otherContourIndex++)
                {
                    IBoundedParameterizedCurve[] otherContour = contours[otherContourIndex];

                    for (int curveIndex = 0; curveIndex < contour.Length; curveIndex++)
                    {
                        for (int otherCurveIndex = 0; otherCurveIndex < otherContour.Length; otherCurveIndex++)
                        {
                            if (CurvesIntersect(contour[curveIndex], otherContour[otherCurveIndex]))
                                throw new ArgumentException("Region contours must not intersect or touch each other.", parameterName);
                        }
                    }
                }
            }
        }

        private static bool CurvesIntersect(IBoundedParameterizedCurve left, IBoundedParameterizedCurve right)
        {
            if (left is Segment leftSegment)
            {
                if (right is Segment rightSegment)
                    return SegmentsIntersect(leftSegment, rightSegment);

                if (right is Arc rightArc)
                    return SegmentIntersectsArc(leftSegment, rightArc);
            }

            if (left is Arc leftArc)
            {
                if (right is Segment rightSegment)
                    return SegmentIntersectsArc(rightSegment, leftArc);

                if (right is Arc rightArc)
                    return ArcsIntersect(leftArc, rightArc);
            }

            return left.Distance(right.StartPoint) <= GeometryConstants.GeometryEpsilon ||
                left.Distance(right.EndPoint) <= GeometryConstants.GeometryEpsilon ||
                right.Distance(left.StartPoint) <= GeometryConstants.GeometryEpsilon ||
                right.Distance(left.EndPoint) <= GeometryConstants.GeometryEpsilon;
        }

        private static bool SegmentsIntersect(Segment left, Segment right)
        {
            VectorXY p = left.StartPoint;
            VectorXY r = left.EndPoint - left.StartPoint;
            VectorXY q = right.StartPoint;
            VectorXY s = right.EndPoint - right.StartPoint;

            if (r.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
                return PointOnSegment(p, right);

            if (s.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
                return PointOnSegment(q, left);

            float det = VectorXY.Cross(r, s);
            VectorXY delta = q - p;

            if (det.IsAlmostZero())
            {
                if (!VectorXY.Cross(delta, r).IsAlmostZero())
                    return false;

                float rr = VectorXY.Dot(r, r);
                float t0 = VectorXY.Dot(q - p, r) / rr;
                float t1 = VectorXY.Dot(q + s - p, r) / rr;

                if (t0 > t1)
                {
                    float swap = t0;
                    t0 = t1;
                    t1 = swap;
                }

                return t0 <= 1f + GeometryConstants.GeometryEpsilon &&
                    t1 >= -GeometryConstants.GeometryEpsilon;
            }

            float t = VectorXY.Cross(delta, s) / det;
            float u = VectorXY.Cross(delta, r) / det;

            return IsUnitIntervalCoordinate(t) && IsUnitIntervalCoordinate(u);
        }

        private static bool SegmentIntersectsArc(Segment segment, Arc arc)
        {
            if (arc.Radius <= GeometryConstants.GeometryEpsilon)
                return PointOnSegment(arc.Center, segment);

            VectorXY start = segment.StartPoint;
            VectorXY direction = segment.EndPoint - segment.StartPoint;

            if (direction.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
                return PointOnArc(start, arc);

            VectorXY startToCenter = start - arc.Center;
            float a = VectorXY.Dot(direction, direction);
            float b = 2f * VectorXY.Dot(startToCenter, direction);
            float c = VectorXY.Dot(startToCenter, startToCenter) - arc.Radius * arc.Radius;
            float discriminant = b * b - 4f * a * c;

            if (discriminant < -GeometryConstants.GeometryEpsilon)
                return false;

            if (discriminant < 0f)
                discriminant = 0f;

            float sqrtDiscriminant = MathF.Sqrt(discriminant);
            float denominator = 2f * a;
            float t0 = (-b - sqrtDiscriminant) / denominator;
            float t1 = (-b + sqrtDiscriminant) / denominator;

            return SegmentCircleIntersectionBelongsToArc(start, direction, t0, arc) ||
                SegmentCircleIntersectionBelongsToArc(start, direction, t1, arc);
        }

        private static bool ArcsIntersect(Arc left, Arc right)
        {
            if (left.Radius <= GeometryConstants.GeometryEpsilon)
                return PointOnArc(left.Center, right);

            if (right.Radius <= GeometryConstants.GeometryEpsilon)
                return PointOnArc(right.Center, left);

            VectorXY centerDelta = right.Center - left.Center;
            float centerDistance = centerDelta.Length;

            if (centerDistance <= GeometryConstants.GeometryEpsilon)
            {
                if (!left.Radius.AlmostEquals(right.Radius))
                    return false;

                return PointOnArc(left.StartPoint, right) ||
                    PointOnArc(left.EndPoint, right) ||
                    PointOnArc(right.StartPoint, left) ||
                    PointOnArc(right.EndPoint, left);
            }

            if (centerDistance > left.Radius + right.Radius + GeometryConstants.GeometryEpsilon)
                return false;

            if (centerDistance < MathF.Abs(left.Radius - right.Radius) - GeometryConstants.GeometryEpsilon)
                return false;

            float a = (left.Radius * left.Radius - right.Radius * right.Radius + centerDistance * centerDistance) / (2f * centerDistance);
            float hSquared = left.Radius * left.Radius - a * a;

            if (hSquared < -GeometryConstants.GeometryEpsilon)
                return false;

            if (hSquared < 0f)
                hSquared = 0f;

            VectorXY centerDirection = centerDelta / centerDistance;
            VectorXY basePoint = left.Center + a * centerDirection;
            float h = MathF.Sqrt(hSquared);
            VectorXY perpendicular = new VectorXY(-centerDirection.Y, centerDirection.X);
            VectorXY intersection = basePoint + h * perpendicular;

            if (PointOnArc(intersection, left) && PointOnArc(intersection, right))
                return true;

            if (h <= GeometryConstants.GeometryEpsilon)
                return false;

            VectorXY otherIntersection = basePoint - h * perpendicular;
            return PointOnArc(otherIntersection, left) && PointOnArc(otherIntersection, right);
        }

        private static bool SegmentCircleIntersectionBelongsToArc(VectorXY segmentStart, VectorXY segmentDirection, float segmentCoordinate, Arc arc)
        {
            if (!IsUnitIntervalCoordinate(segmentCoordinate))
                return false;

            VectorXY point = segmentStart + segmentCoordinate * segmentDirection;
            return arc.IsWithinAngularRegion(point);
        }

        private static bool PointOnArc(VectorXY point, Arc arc)
        {
            return point.Distance(arc.Center).AlmostEquals(arc.Radius) && arc.IsWithinAngularRegion(point);
        }

        private static bool PointOnSegment(VectorXY point, Segment segment)
        {
            VectorXY segmentVector = segment.EndPoint - segment.StartPoint;
            VectorXY startToPoint = point - segment.StartPoint;

            if (segmentVector.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
                return point.AlmostEquals(segment.StartPoint);

            if (!VectorXY.Cross(segmentVector, startToPoint).IsAlmostZero())
                return false;

            float dot = VectorXY.Dot(startToPoint, segmentVector);
            return dot >= -GeometryConstants.GeometryEpsilon &&
                dot <= segmentVector.SquaredLength + GeometryConstants.GeometryEpsilon;
        }

        private static bool IsUnitIntervalCoordinate(float value)
        {
            return value >= -GeometryConstants.GeometryEpsilon &&
                value <= 1f + GeometryConstants.GeometryEpsilon;
        }
    }
}
