using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a finite line segment in two-dimensional space.
    /// </summary>
    public readonly struct Segment : IParameterizedProjectableCurve, IEquatable<Segment>
    {
        private readonly VectorXY _startPoint;
        private readonly VectorXY _endPoint;

        private readonly bool _includesStartPoint;
        private readonly bool _includesEndPoint;

        /// <summary>
        /// Initializes a new segment with both endpoints included.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        public Segment(VectorXY startPoint, VectorXY endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            _includesStartPoint = true;
            _includesEndPoint = true;
        }

        /// <summary>
        /// Initializes a new segment with explicit endpoint inclusion.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="includesStartPoint">Whether the start point belongs to the segment.</param>
        /// <param name="includesEndPoint">Whether the end point belongs to the segment.</param>
        public Segment(VectorXY startPoint, VectorXY endPoint, bool includesStartPoint, bool includesEndPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            _includesStartPoint = includesStartPoint;
            _includesEndPoint = includesEndPoint;
        }

        /// <summary>
        /// Gets the start point.
        /// </summary>
        public VectorXY StartPoint => _startPoint;

        /// <summary>
        /// Gets the end point.
        /// </summary>
        public VectorXY EndPoint => _endPoint;

        /// <summary>
        /// Gets a value indicating whether the start point belongs to the segment.
        /// </summary>
        public bool IncludesStartPoint => _includesStartPoint;

        /// <summary>
        /// Gets a value indicating whether the end point belongs to the segment.
        /// </summary>
        public bool IncludesEndPoint => _includesEndPoint;

        /// <summary>
        /// Returns point intersections with the specified ray. If the ray is collinear with the segment
        /// and their overlap is not a single point, returns the first included point encountered along the ray.
        /// If the overlap starts at an excluded endpoint, no first point exists and the result is empty.
        /// </summary>
        /// <param name="ray">The ray to intersect with this segment.</param>
        /// <returns>The intersection points in the forward direction of the ray.</returns>
        public List<VectorXY> GetRayIntersections(Ray ray)
        {
            List<VectorXY> intersections = new List<VectorXY>();

            VectorXY rayDir = ray.Direction;
            VectorXY segDir = EndPoint - StartPoint;
            VectorXY delta = StartPoint - ray.Origin;

            float det = rayDir.X * (-segDir.Y) + rayDir.Y * segDir.X;

            if (det.IsAlmostZero())
            {
                AddFirstCollinearIntersection(ray, intersections);
                return intersections;
            }

            float t = (delta.X * -segDir.Y + delta.Y * segDir.X) / det;
            float u = (rayDir.X * delta.Y - rayDir.Y * delta.X) / det;

            if (t >= 0f && u >= 0f && u <= 1f)
            {
                VectorXY intersection = ray.Origin + t * rayDir;

                if (intersection.AlmostEquals(StartPoint))
                {
                    if (IncludesStartPoint)
                        intersections.AddDistinct(intersection);
                }
                else if (intersection.AlmostEquals(EndPoint))
                {
                    if (IncludesEndPoint)
                        intersections.AddDistinct(intersection);
                }
                else
                {
                    intersections.AddDistinct(intersection);
                }
            }

            return intersections;
        }

        private void AddFirstCollinearIntersection(Ray ray, List<VectorXY> intersections)
        {
            VectorXY segDir = EndPoint - StartPoint;
            VectorXY originToStart = StartPoint - ray.Origin;

            if (!VectorXY.Cross(originToStart, ray.Direction).IsAlmostZero())
                return;

            if (segDir.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
            {
                if ((IncludesStartPoint || IncludesEndPoint) && IsPointOnRay(StartPoint, ray, out _))
                    intersections.AddDistinct(StartPoint);

                return;
            }

            float tStart = VectorXY.Dot(StartPoint - ray.Origin, ray.Direction);
            float tEnd = VectorXY.Dot(EndPoint - ray.Origin, ray.Direction);

            VectorXY startPoint;
            bool startIncluded;
            float startT;
            float endT;

            if (tStart <= tEnd)
            {
                startPoint = StartPoint;
                startIncluded = IncludesStartPoint;
                startT = tStart;
                endT = tEnd;
            }
            else
            {
                startPoint = EndPoint;
                startIncluded = IncludesEndPoint;
                startT = tEnd;
                endT = tStart;
            }

            if (endT < -GeometryConstants.GeometryEpsilon)
                return;

            float firstT = MathF.Max(0f, startT);
            if (firstT > endT + GeometryConstants.GeometryEpsilon)
                return;

            if (firstT.IsAlmostZero())
            {
                if (IncludesPoint(ray.Origin))
                    intersections.AddDistinct(ray.Origin);

                return;
            }

            if (startIncluded)
                intersections.AddDistinct(startPoint);
        }

        private bool IncludesPoint(VectorXY point)
        {
            VectorXY segmentVector = EndPoint - StartPoint;
            VectorXY startToPoint = point - StartPoint;

            if (!VectorXY.Cross(segmentVector, startToPoint).IsAlmostZero())
                return false;

            float dot = VectorXY.Dot(startToPoint, segmentVector);
            if (dot < -GeometryConstants.GeometryEpsilon)
                return false;

            if (dot > segmentVector.SquaredLength + GeometryConstants.GeometryEpsilon)
                return false;

            if (point.AlmostEquals(StartPoint) && !IncludesStartPoint)
                return false;

            if (point.AlmostEquals(EndPoint) && !IncludesEndPoint)
                return false;

            return true;
        }

        private static bool IsPointOnRay(VectorXY point, Ray ray, out float t)
        {
            VectorXY toPoint = point - ray.Origin;
            t = VectorXY.Dot(toPoint, ray.Direction);

            if (t < -GeometryConstants.GeometryEpsilon)
                return false;

            return VectorXY.Cross(toPoint, ray.Direction).IsAlmostZero();
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Segment other && Equals(other);

        /// <summary>
        /// Indicates whether this segment has the same endpoints and endpoint-inclusion flags as another segment.
        /// </summary>
        /// <param name="other">The segment to compare with this segment.</param>
        /// <returns><see langword="true"/> if both segments are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Segment other) =>
            StartPoint.Equals(other.StartPoint) &&
            EndPoint.Equals(other.EndPoint) &&
            IncludesStartPoint == other.IncludesStartPoint &&
            IncludesEndPoint == other.IncludesEndPoint;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(StartPoint, EndPoint, IncludesStartPoint, IncludesEndPoint);

        /// <inheritdoc/>
        public override string ToString() => $"({StartPoint} - {EndPoint})";

        /// <summary>
        /// Returns the shortest distance from the specified point to this segment.
        /// </summary>
        /// <param name="point">The point to measure from.</param>
        /// <returns>The distance to this segment.</returns>
        public float Distance(VectorXY point)
        {
            return Project(point).Distance;
        }

        /// <summary>
        /// Projects the specified point onto this segment.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point and distance to this segment.</returns>
        public CurveProjection Project(VectorXY point)
        {
            var projection = ProjectWithParameter(point);
            return new CurveProjection(projection.ProjectedPoint, projection.Distance);
        }

        /// <summary>
        /// Projects the specified point onto this segment and reports the segment length coordinate.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point, segment length coordinate, and distance to this segment.</returns>
        public ParameterizedCurveProjection ProjectWithParameter(VectorXY point)
        {
            VectorXY segmentVector = EndPoint - StartPoint;
            VectorXY startToPoint = point - StartPoint;

            float segmentLengthSquared = segmentVector.SquaredLength;
            if (segmentLengthSquared <= GeometryConstants.GeometryEpsilonSquared)
                return new ParameterizedCurveProjection(StartPoint, 0f, point.Distance(StartPoint));

            float normalizedParameter = VectorXY.Dot(startToPoint, segmentVector) / segmentLengthSquared;

            if (normalizedParameter < 0f)
                normalizedParameter = 0f;
            else if (normalizedParameter > 1f)
                normalizedParameter = 1f;

            VectorXY projection = StartPoint + normalizedParameter * segmentVector;
            float curveCoordinate = normalizedParameter * MathF.Sqrt(segmentLengthSquared);
            return new ParameterizedCurveProjection(projection, curveCoordinate, point.Distance(projection));
        }

        /// <summary>
        /// Indicates whether two segments are equal.
        /// </summary>
        /// <param name="left">The first segment.</param>
        /// <param name="right">The second segment.</param>
        /// <returns><see langword="true"/> if the segments are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Segment left, Segment right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two segments are different.
        /// </summary>
        /// <param name="left">The first segment.</param>
        /// <param name="right">The second segment.</param>
        /// <returns><see langword="true"/> if the segments are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Segment left, Segment right) => !(left == right);

        /// <summary>
        /// Translates a segment by a vector.
        /// </summary>
        /// <param name="left">The segment to translate.</param>
        /// <param name="right">The translation vector.</param>
        /// <returns>The translated segment.</returns>
        public static Segment operator +(Segment left, VectorXY right) => new Segment(
            left.StartPoint + right,
            left.EndPoint + right,
            left.IncludesStartPoint,
            left.IncludesEndPoint);

        /// <summary>
        /// Translates a segment by the negated vector.
        /// </summary>
        /// <param name="left">The segment to translate.</param>
        /// <param name="right">The translation vector to subtract.</param>
        /// <returns>The translated segment.</returns>
        public static Segment operator -(Segment left, VectorXY right) => new Segment(
            left.StartPoint - right,
            left.EndPoint - right,
            left.IncludesStartPoint,
            left.IncludesEndPoint);
    }
}
