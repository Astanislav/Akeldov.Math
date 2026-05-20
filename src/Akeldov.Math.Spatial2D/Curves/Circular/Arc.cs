using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a circular arc in two-dimensional space.
    /// </summary>
    [Serializable]
    public readonly struct Arc : IFinitePath, IEquatable<Arc>
    {
        private readonly VectorXY _center;
        private readonly float _radius;
        private readonly float _startAngle;
        private readonly float _endAngle;
        private readonly bool _isFullCircle;

        /// <summary>
        /// Creates an arc from <paramref name="startAngle"/> to <paramref name="endAngle"/>.
        /// Equal input angles represent a zero-length arc. An end angle one full turn after the start angle
        /// represents a full circle even though both angles normalize to the same value.
        /// </summary>
        /// <param name="center">The center of the source circle.</param>
        /// <param name="radius">The radius of the source circle.</param>
        /// <param name="startAngle">The start angle in radians.</param>
        /// <param name="endAngle">The end angle in radians.</param>
        public Arc(VectorXY center, float radius, float startAngle, float endAngle)
        {
            if (!center.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(center), "Arc center coordinates must be finite.");

            if (radius < 0f || float.IsNaN(radius) || float.IsInfinity(radius))
                throw new ArgumentOutOfRangeException(nameof(radius), "Arc radius must be finite and non-negative.");

            if (float.IsNaN(startAngle) || float.IsInfinity(startAngle))
                throw new ArgumentOutOfRangeException(nameof(startAngle), "Arc start angle must be finite.");

            if (float.IsNaN(endAngle) || float.IsInfinity(endAngle))
                throw new ArgumentOutOfRangeException(nameof(endAngle), "Arc end angle must be finite.");

            _center = center;
            _radius = radius;
            _startAngle = startAngle.NormalizeAngleRad();
            _endAngle = endAngle.NormalizeAngleRad();
            _isFullCircle = IsFullTurn(startAngle, endAngle);
        }

        /// <summary>
        /// Gets the center of the source circle.
        /// </summary>
        public VectorXY Center => _center;

        /// <summary>
        /// Gets the radius of the source circle.
        /// </summary>
        public float Radius => _radius;

        /// <summary>
        /// Gets the normalized start angle in radians.
        /// </summary>
        public float StartAngle => _startAngle;

        /// <summary>
        /// Gets the normalized end angle in radians.
        /// </summary>
        public float EndAngle => _endAngle;

        /// <summary>
        /// Gets a value indicating whether this arc represents a full circle.
        /// </summary>
        public bool IsFullCircle => _isFullCircle;

        /// <summary>
        /// Gets the point at the start angle of this arc.
        /// </summary>
        public VectorXY StartPoint => GetPointAtAngle(_startAngle);

        /// <summary>
        /// Gets the point at the end angle of this arc.
        /// </summary>
        public VectorXY EndPoint => GetPointAtAngle(_endAngle);

        /// <summary>
        /// Gets the first endpoint.
        /// </summary>
        public VectorXY EndpointA => StartPoint;

        /// <summary>
        /// Gets the second endpoint.
        /// </summary>
        public VectorXY EndpointB => EndPoint;

        /// <summary>
        /// Gets the arc length.
        /// </summary>
        public float Length => GetArcLength();

        /// <summary>
        /// Determines whether the direction from this arc's center to the specified point lies within this arc's angular region.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns><see langword="true"/> if the point lies within this arc's angular region; otherwise, <see langword="false"/>.</returns>
        public bool IsWithinAngularRegion(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            VectorXY toPoint = point - Center;
            if (toPoint.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
                return ContainsAngle(_startAngle);

            float angle = MathF.Atan2(toPoint.Y, toPoint.X).NormalizeAngleRad();
            return ContainsAngle(angle);
        }

        /// <summary>
        /// Returns point intersections between this arc and the specified ray.
        /// </summary>
        /// <param name="ray">The ray to intersect with this arc.</param>
        /// <param name="geometryEpsilon">The geometry comparison tolerance in world coordinate units.</param>
        /// <returns>A new mutable list of intersection points in the forward direction of the ray, owned by the caller.</returns>
        public List<VectorXY> GetRayIntersections(Ray ray, float geometryEpsilon = 1E-06F)
        {
            GeometryConstants.ValidateGeometryEpsilon(geometryEpsilon, nameof(geometryEpsilon));

            var intersections = new List<VectorXY>();

            if (_radius <= geometryEpsilon)
            {
                VectorXY toCenter = _center - ray.Origin;
                if (VectorXY.Dot(toCenter, ray.Direction) >= -geometryEpsilon &&
                    VectorXY.Cross(toCenter, ray.Direction).IsAlmostZero(geometryEpsilon))
                {
                    intersections.AddDistinct(_center, geometryEpsilon);
                }

                return intersections;
            }

            VectorXY direction = ray.Direction;
            VectorXY originToCenter = ray.Origin - _center;

            float b = 2f * VectorXY.Dot(originToCenter, direction);
            float c = originToCenter.SquaredLength - _radius * _radius;
            float discriminant = b * b - 4f * c;

            if (discriminant < -geometryEpsilon)
                return intersections;

            if (discriminant < 0f)
                discriminant = 0f;

            float sqrtDiscriminant = MathF.Sqrt(discriminant);
            float t1 = (-b - sqrtDiscriminant) / 2f;
            float t2 = (-b + sqrtDiscriminant) / 2f;

            AddRayCircleIntersectionIfOnArc(ray, t1, intersections, geometryEpsilon);

            if (!t2.AlmostEquals(t1, geometryEpsilon))
                AddRayCircleIntersectionIfOnArc(ray, t2, intersections, geometryEpsilon);

            return intersections;
        }

        /// <summary>
        /// Returns the shortest distance from the specified point to this arc.
        /// </summary>
        /// <param name="point">The point to measure from.</param>
        /// <returns>The distance to this arc.</returns>
        public float Distance(VectorXY point)
        {
            return Project(point).Distance;
        }

        /// <summary>
        /// Projects the specified point onto this arc.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point and distance to this arc.</returns>
        public CurveProjection Project(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            VectorXY toPoint = point - _center;

            if (_radius <= GeometryConstants.GeometryEpsilon || toPoint.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
            {
                VectorXY start = StartPoint;
                return new CurveProjection(start, point.Distance(start));
            }

            float angleToPoint = MathF.Atan2(toPoint.Y, toPoint.X).NormalizeAngleRad();

            if (ContainsAngle(angleToPoint))
            {
                VectorXY projected = _center + toPoint.Normalize() * _radius;
                return new CurveProjection(projected, point.Distance(projected));
            }

            VectorXY arcStart = StartPoint;
            VectorXY arcEnd = EndPoint;

            float distStart = point.Distance(arcStart);
            float distEnd = point.Distance(arcEnd);

            if (distStart <= distEnd)
                return new CurveProjection(arcStart, distStart);

            return new CurveProjection(arcEnd, distEnd);
        }

        /// <summary>
        /// Projects the specified point onto this arc and reports the arc length coordinate.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point, arc length coordinate, and distance to this arc.</returns>
        public ParameterizedCurveProjection ProjectWithParameter(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            CurveProjection projection = Project(point);
            float curveCoordinate = GetCurveCoordinate(projection.ProjectedPoint);

            return new ParameterizedCurveProjection(
                projection.ProjectedPoint,
                curveCoordinate,
                projection.Distance);
        }

        /// <summary>
        /// Returns the point at the specified arc length coordinate.
        /// </summary>
        /// <param name="curveCoordinate">The finite curve coordinate in world coordinate units.</param>
        /// <returns>The point on this arc.</returns>
        public VectorXY GetPoint(float curveCoordinate)
        {
            if (float.IsNaN(curveCoordinate) || float.IsInfinity(curveCoordinate))
                throw new ArgumentOutOfRangeException(nameof(curveCoordinate), "Curve coordinate must be finite.");

            float length = Length;
            if (curveCoordinate < 0f || curveCoordinate > length)
                throw new ArgumentOutOfRangeException(nameof(curveCoordinate), "Curve coordinate must lie within the arc length.");

            if (_radius <= GeometryConstants.GeometryEpsilon)
                return StartPoint;

            float angle = (_startAngle + curveCoordinate / _radius).NormalizeAngleRad();
            return GetPointAtAngle(angle);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Arc other && Equals(other);

        /// <summary>
        /// Indicates whether this arc has the same center, radius, angles, and full-circle flag as another arc.
        /// </summary>
        /// <param name="other">The arc to compare with this arc.</param>
        /// <returns><see langword="true"/> if both arcs are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Arc other)
        {
            return Center.Equals(other.Center) &&
                Radius.Equals(other.Radius) &&
                StartAngle.Equals(other.StartAngle) &&
                EndAngle.Equals(other.EndAngle) &&
                IsFullCircle == other.IsFullCircle;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Center, Radius, StartAngle, EndAngle, IsFullCircle);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Arc(center: {Center}, radius: {Radius}, rad: {StartAngle} - {EndAngle}, fullCircle: {IsFullCircle})";
        }

        /// <summary>
        /// Indicates whether two arcs are equal.
        /// </summary>
        /// <param name="left">The first arc.</param>
        /// <param name="right">The second arc.</param>
        /// <returns><see langword="true"/> if the arcs are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Arc left, Arc right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two arcs are different.
        /// </summary>
        /// <param name="left">The first arc.</param>
        /// <param name="right">The second arc.</param>
        /// <returns><see langword="true"/> if the arcs are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Arc left, Arc right)
        {
            return !(left == right);
        }

        private void AddRayCircleIntersectionIfOnArc(
            Ray ray,
            float rayCoordinate,
            List<VectorXY> intersections,
            float geometryEpsilon)
        {
            if (rayCoordinate < -geometryEpsilon)
                return;

            VectorXY point = ray.Origin + ray.Direction * rayCoordinate;
            if (IsWithinAngularRegion(point))
                intersections.AddDistinct(point, geometryEpsilon);
        }

        private float GetArcLength()
        {
            if (IsFullCircle)
                return 2f * MathF.PI * _radius;

            return PositiveAngleDelta(_startAngle, _endAngle) * _radius;
        }

        private float GetCurveCoordinate(VectorXY point)
        {
            if (_radius <= GeometryConstants.GeometryEpsilon)
                return 0f;

            VectorXY toPoint = point - _center;
            if (toPoint.SquaredLength <= GeometryConstants.GeometryEpsilonSquared)
                return 0f;

            float angle = MathF.Atan2(toPoint.Y, toPoint.X).NormalizeAngleRad();

            if (!ContainsAngle(angle))
                return point.Distance(StartPoint) <= point.Distance(EndPoint) ? 0f : Length;

            return PositiveAngleDelta(_startAngle, angle) * _radius;
        }

        private bool ContainsAngle(float angle)
        {
            return IsFullCircle || angle.IsAngleWithinArc(_startAngle, _endAngle);
        }

        private VectorXY GetPointAtAngle(float angle)
        {
            return new VectorXY(
                _center.X + _radius * MathF.Cos(angle),
                _center.Y + _radius * MathF.Sin(angle));
        }

        private static float PositiveAngleDelta(float from, float to)
        {
            float delta = to - from;
            if (delta < 0f)
                delta += 2f * MathF.PI;

            return delta;
        }

        private static bool IsFullTurn(float startAngle, float endAngle)
        {
            float delta = endAngle - startAngle;
            if (MathF.Abs(delta) <= GeometryConstants.GeometryEpsilon)
                return false;

            float turns = delta / (2f * MathF.PI);
            return turns.AlmostEquals(MathF.Round(turns));
        }
    }
}
