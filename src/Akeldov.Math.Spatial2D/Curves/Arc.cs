using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a closed circular arc in two-dimensional space.
    /// </summary>
    [Serializable]
    public readonly struct Arc : IProjectableCurve, IEquatable<Arc>
    {
        private readonly VectorXY _center;
        private readonly float _radius;
        private readonly float _startAngle;
        private readonly float _endAngle;
        private readonly bool _isFullCircle;

        /// <summary>
        /// Creates a closed arc from <paramref name="startAngleRad"/> to <paramref name="stopAngleRad"/>.
        /// Equal input angles represent a zero-length arc. A stop angle one full turn after the start angle
        /// represents a full circle even though both angles normalize to the same value.
        /// </summary>
        /// <param name="center">The center of the source circle.</param>
        /// <param name="radius">The radius of the source circle.</param>
        /// <param name="startAngleRad">The start angle in radians.</param>
        /// <param name="stopAngleRad">The stop angle in radians.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is negative, NaN, or infinite, or when an angle is NaN or infinite.</exception>
        public Arc(VectorXY center, float radius, float startAngleRad, float stopAngleRad)
        {
            if (radius < 0f || float.IsNaN(radius) || float.IsInfinity(radius))
                throw new ArgumentOutOfRangeException(nameof(radius), "Arc radius must be finite and non-negative.");

            if (float.IsNaN(startAngleRad) || float.IsInfinity(startAngleRad))
                throw new ArgumentOutOfRangeException(nameof(startAngleRad), "Arc start angle must be finite.");

            if (float.IsNaN(stopAngleRad) || float.IsInfinity(stopAngleRad))
                throw new ArgumentOutOfRangeException(nameof(stopAngleRad), "Arc stop angle must be finite.");

            _center = center;
            _radius = radius;
            _startAngle = startAngleRad.NormalizeAngleRad();
            _endAngle = stopAngleRad.NormalizeAngleRad();
            _isFullCircle = IsFullTurn(startAngleRad, stopAngleRad);
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
        /// Gets the normalized start angle in degrees.
        /// </summary>
        public float StartAngleDeg => _startAngle * Constants.Rad2Deg;

        /// <summary>
        /// Gets the normalized end angle in degrees.
        /// </summary>
        public float EndAngleDeg => _endAngle * Constants.Rad2Deg;

        /// <summary>
        /// Gets a value indicating whether this arc represents a full circle.
        /// </summary>
        public bool IsFullCircle => _isFullCircle;

        /// <summary>
        /// Returns the point at the start angle of this arc.
        /// </summary>
        /// <returns>The arc start point.</returns>
        public VectorXY GetStartPoint()
        {
            return new VectorXY(
                _center.X + _radius * MathF.Cos(_startAngle),
                _center.Y + _radius * MathF.Sin(_startAngle)
            );
        }

        /// <summary>
        /// Returns the point at the end angle of this arc.
        /// </summary>
        /// <returns>The arc end point.</returns>
        public VectorXY GetEndPoint()
        {
            return new VectorXY(
                _center.X + _radius * MathF.Cos(_endAngle),
                _center.Y + _radius * MathF.Sin(_endAngle)
            );
        }

        /// <summary>
        /// Determines whether the specified point's angular position lies within this arc's angular span.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns><see langword="true"/> if the point's angular position lies within this arc; otherwise, <see langword="false"/>.</returns>
        public bool ContainsAngularPosition(VectorXY point)
        {
            VectorXY toPoint = (point - Center).Normalize();
            float angle = MathF.Atan2(toPoint.Y, toPoint.X).NormalizeAngleRad();

            return ContainsAngle(angle);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Arc other && Equals(other);

        /// <summary>
        /// Indicates whether this arc has the same center, radius, angles, and full-circle flag as another arc.
        /// </summary>
        /// <param name="other">The arc to compare with this arc.</param>
        /// <returns><see langword="true"/> if both arcs are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Arc other) =>
            Center.Equals(other.Center) &&
            Radius.Equals(other.Radius) &&
            StartAngle.Equals(other.StartAngle) &&
            EndAngle.Equals(other.EndAngle) &&
            IsFullCircle == other.IsFullCircle;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Center, Radius, StartAngle, EndAngle, IsFullCircle);

        /// <inheritdoc/>
        public override string ToString() => $"Arc(center: {Center}, radius: {Radius}, rad: {StartAngle} - {EndAngle}, fullCircle: {IsFullCircle})";

        /// <summary>
        /// Returns a string representation of this arc with angles in degrees.
        /// </summary>
        /// <returns>A string representation of this arc with degree angles.</returns>
        public string ToStringDeg() => $"Arc(center: {Center}, radius: {Radius}, deg: {StartAngleDeg} - {EndAngleDeg}, fullCircle: {IsFullCircle})";

        /// <summary>
        /// Returns point intersections between this arc and the specified ray.
        /// </summary>
        /// <param name="ray">The ray to intersect with this arc.</param>
        /// <returns>The intersection points in the forward direction of the ray.</returns>
        public List<VectorXY> RayIntersections(Ray ray)
        {
            var intersections = new List<VectorXY>();

            if (_radius <= GeometryConstants.GeometryEpsilon)
            {
                VectorXY toCenter = _center - ray.Origin;
                if (VectorXY.Dot(toCenter, ray.Dir) >= -GeometryConstants.GeometryEpsilon &&
                    VectorXY.Cross(toCenter, ray.Dir).IsAlmostZero())
                {
                    intersections.AddDistinct(_center);
                }

                return intersections;
            }

            var circleIntersections = new List<VectorXY>();
            VectorXY dir = ray.Dir;
            var circle = new Circle(Center, Radius);
            VectorXY f = ray.Origin - circle.Center;

            float a = 1f;
            float b = 2f * VectorXY.Dot(f, dir);
            float c = f.SQRLength - circle.Radius * circle.Radius;

            float discriminant = b * b - 4f * a * c;

            if (discriminant < 0f)
                return circleIntersections;

            float sqrtD = MathF.Sqrt(discriminant);

            float t1 = (-b - sqrtD) / (2f * a);
            float t2 = (-b + sqrtD) / (2f * a);

            if (t1 >= 0f)
            {
                VectorXY point1 = ray.Origin + dir * t1;
                circleIntersections.AddDistinct(point1);

                if (ContainsAngularPosition(point1))
                    intersections.AddDistinct(point1);
            }

            if (t2 >= 0f)
            {
                VectorXY point2 = ray.Origin + dir * t2;
                circleIntersections.AddDistinct(point2);

                if (ContainsAngularPosition(point2))
                    intersections.AddDistinct(point2);
            }

            if (circleIntersections.Count == 0)
                return circleIntersections;

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
        /// <returns>The projection point, arc parameter, and distance to this arc.</returns>
        public CurveProjection Project(VectorXY point)
        {
            VectorXY toPoint = point - _center;

            if (_radius <= GeometryConstants.GeometryEpsilon || toPoint.SQRLength <= GeometryConstants.GeometryEpsilonSquared)
            {
                VectorXY start = GetStartPoint();
                return new CurveProjection(start, 0f, point.Distance(start));
            }

            float angleToPoint = MathF.Atan2(toPoint.Y, toPoint.X).NormalizeAngleRad();

            if (ContainsAngle(angleToPoint))
            {
                VectorXY projected = _center + toPoint.Normalize() * _radius;
                float parameter = GetParameter(angleToPoint);
                return new CurveProjection(projected, parameter, point.Distance(projected));
            }

            VectorXY arcStart = GetStartPoint();
            VectorXY arcEnd = GetEndPoint();

            float distStart = point.Distance(arcStart);
            float distEnd = point.Distance(arcEnd);

            if (distStart <= distEnd)
                return new CurveProjection(arcStart, 0f, distStart);

            return new CurveProjection(arcEnd, 1f, distEnd);
        }

        /// <summary>
        /// Indicates whether two arcs are equal.
        /// </summary>
        /// <param name="left">The first arc.</param>
        /// <param name="right">The second arc.</param>
        /// <returns><see langword="true"/> if the arcs are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Arc left, Arc right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two arcs are different.
        /// </summary>
        /// <param name="left">The first arc.</param>
        /// <param name="right">The second arc.</param>
        /// <returns><see langword="true"/> if the arcs are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Arc left, Arc right) => !(left == right);

        private float GetParameter(float angle)
        {
            if (IsFullCircle)
                return PositiveAngleDelta(_startAngle, angle) / (2f * MathF.PI);

            float span = PositiveAngleDelta(_startAngle, _endAngle);
            if (span <= GeometryConstants.GeometryEpsilon)
                return 0f;

            return PositiveAngleDelta(_startAngle, angle) / span;
        }

        private bool ContainsAngle(float angle)
        {
            return IsFullCircle || angle.IsAngleWithinArc(_startAngle, _endAngle);
        }

        private static float PositiveAngleDelta(float from, float to)
        {
            float delta = to - from;
            if (delta < 0f)
                delta += 2f * MathF.PI;

            return delta;
        }

        private static bool IsFullTurn(float startAngle, float stopAngle)
        {
            float delta = stopAngle - startAngle;
            if (MathF.Abs(delta) <= GeometryConstants.GeometryEpsilon)
                return false;

            float turns = delta / (2f * MathF.PI);
            return turns.AlmostEquals(MathF.Round(turns));
        }

    }
}
