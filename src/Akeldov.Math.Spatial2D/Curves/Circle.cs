using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a circle in two-dimensional space.
    /// </summary>
    public readonly struct Circle : ICurve, IEquatable<Circle>
    {
        private readonly VectorXY _center;
        private readonly float _radius;

        /// <summary>
        /// Initializes a new circle with the specified center and radius.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The circle radius.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="radius"/> is negative.</exception>
        public Circle(VectorXY center, float radius)
        {
            if (radius < 0f)
                throw new ArgumentOutOfRangeException(nameof(radius));

            _center = center;
            _radius = radius;
        }

        /// <summary>
        /// Gets the center of the circle.
        /// </summary>
        public VectorXY Center => _center;

        /// <summary>
        /// Gets the circle radius.
        /// </summary>
        public float Radius => _radius;

        /// <summary>
        /// Returns the shortest distance from the specified point to the circle circumference.
        /// </summary>
        /// <param name="point">The point to measure from.</param>
        /// <returns>The absolute distance to the circle circumference.</returns>
        public float Distance(VectorXY point)
        {
            float distToCenter = (point - _center).Length;
            return MathF.Abs(distToCenter - _radius);
        }

        /// <summary>
        /// Returns point intersections between this circle and the specified ray.
        /// </summary>
        /// <param name="ray">The ray to intersect with the circle.</param>
        /// <returns>The intersection points in the forward direction of the ray.</returns>
        public List<VectorXY> RayIntersections(Ray ray)
        {
            List<VectorXY> intersections = new List<VectorXY>();

            VectorXY d = ray.Dir;
            VectorXY f = ray.Origin - _center;

            float a = 1f;
            float b = 2 * VectorXY.Dot(f, d);
            float c = f.SQRLength - _radius * _radius;

            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                return intersections;
            }

            discriminant = MathF.Sqrt(discriminant);

            float t1 = (-b - discriminant) / (2 * a);
            float t2 = (-b + discriminant) / (2 * a);

            if (t1 >= 0)
            {
                intersections.AddDistinct(ray.Origin + d * t1);
            }

            if (t2 >= 0 && !t2.AlmostEquals(t1))
            {
                intersections.AddDistinct(ray.Origin + d * t2);
            }

            return intersections;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Circle other && Equals(other);

        /// <summary>
        /// Indicates whether this circle has the same center and radius as another circle.
        /// </summary>
        /// <param name="other">The circle to compare with this circle.</param>
        /// <returns><see langword="true"/> if both circles are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Circle other) => Center.Equals(other.Center) && Radius.Equals(other.Radius);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Center, Radius);

        /// <inheritdoc/>
        public override string ToString() => $"Circle(center: {Center}, radius: {Radius})";

        /// <summary>
        /// Indicates whether two circles are equal.
        /// </summary>
        /// <param name="left">The first circle.</param>
        /// <param name="right">The second circle.</param>
        /// <returns><see langword="true"/> if the circles are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Circle left, Circle right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two circles are different.
        /// </summary>
        /// <param name="left">The first circle.</param>
        /// <param name="right">The second circle.</param>
        /// <returns><see langword="true"/> if the circles are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Circle left, Circle right) => !(left == right);
    }
}
