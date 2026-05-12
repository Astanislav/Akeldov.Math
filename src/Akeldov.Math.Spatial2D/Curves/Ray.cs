using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a half-line that starts at an origin and extends in one direction.
    /// </summary>
    public readonly struct Ray : IProjectableCurve
    {
        private readonly VectorXY _origin;
        private readonly float _angleRad;
        private readonly VectorXY _direction;

        /// <summary>
        /// Initializes a new ray that starts at the specified origin and points along the positive X axis.
        /// </summary>
        /// <param name="origin">The ray origin.</param>
        public Ray(VectorXY origin)
        {
            _origin = origin;
            _angleRad = 0;
            _direction = new VectorXY(1, 0);
        }

        /// <summary>
        /// Initializes a new ray that starts at the specified origin and points in the specified angle.
        /// </summary>
        /// <param name="origin">The ray origin.</param>
        /// <param name="angleRad">The ray direction angle in radians.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="angleRad"/> is NaN or infinite.</exception>
        public Ray(VectorXY origin, float angleRad)
        {
            if (float.IsNaN(angleRad) || float.IsInfinity(angleRad))
                throw new ArgumentOutOfRangeException(nameof(angleRad), "Ray angle must be finite.");

            _origin = origin;
            _angleRad = angleRad;
            _direction = new VectorXY(MathF.Cos(angleRad), MathF.Sin(angleRad));
        }

        /// <summary>
        /// Gets the ray origin.
        /// </summary>
        public VectorXY Origin => _origin;

        /// <summary>
        /// Gets the normalized ray direction.
        /// </summary>
        public VectorXY Direction => _direction;

        /// <summary>
        /// Gets the ray direction angle in radians.
        /// </summary>
        public float AngleRad => _angleRad;

        /// <summary>
        /// Returns the shortest distance from the specified point to this ray.
        /// </summary>
        /// <param name="point">The point to measure from.</param>
        /// <returns>The distance to this ray.</returns>
        public float Distance(VectorXY point)
        {
            return Project(point).Distance;
        }

        /// <summary>
        /// Projects the specified point onto this ray.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point, ray parameter, and distance to this ray.</returns>
        public CurveProjection Project(VectorXY point)
        {
            VectorXY toPoint = point - _origin;
            float t = VectorXY.Dot(toPoint, Direction);

            if (t < 0f)
                t = 0f;

            VectorXY projected = _origin + Direction * t;
            return new CurveProjection(projected, t, point.Distance(projected));
        }

        /// <summary>
        /// Returns point intersections with another ray. If the rays are collinear and overlap,
        /// returns the first point along this ray that belongs to the other ray.
        /// </summary>
        /// <param name="other">The other ray to intersect with this ray.</param>
        /// <returns>The intersection points in the forward direction of this ray.</returns>
        public List<VectorXY> GetRayIntersections(Ray other)
        {
            List<VectorXY> intersections = new List<VectorXY>();

            VectorXY p = _origin;
            VectorXY r = Direction;
            VectorXY q = other._origin;
            VectorXY s = other.Direction;

            float cross = VectorXY.Cross(r, s);

            if (cross.IsAlmostZero())
            {
                AddFirstCollinearIntersection(other, intersections);
                return intersections;
            }

            VectorXY qMinusP = q - p;

            float t = VectorXY.Cross(qMinusP, s) / cross;
            float u = VectorXY.Cross(qMinusP, r) / cross;

            if (t >= 0 && u >= 0)
            {
                VectorXY intersectionPoint = p + r * t;
                intersections.AddDistinct(intersectionPoint);
            }

            return intersections;
        }

        private void AddFirstCollinearIntersection(Ray other, List<VectorXY> intersections)
        {
            VectorXY originDelta = other._origin - _origin;

            if (!VectorXY.Cross(originDelta, Direction).IsAlmostZero())
                return;

            if (IsPointOnRay(_origin, other))
            {
                intersections.AddDistinct(_origin);
                return;
            }

            if (IsPointOnRay(other._origin, this))
                intersections.AddDistinct(other._origin);
        }

        private static bool IsPointOnRay(VectorXY point, Ray ray)
        {
            VectorXY toPoint = point - ray._origin;

            if (VectorXY.Dot(toPoint, ray.Direction) < -GeometryConstants.GeometryEpsilon)
                return false;

            return VectorXY.Cross(toPoint, ray.Direction).IsAlmostZero();
        }
    }
}
