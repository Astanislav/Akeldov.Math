using System;
using System.Runtime.InteropServices;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    /// <summary>
    /// Represents a weighted Voronoi site.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Site : IEquatable<Site>
    {
        /// <summary>
        /// Initializes a new weighted Voronoi site.
        /// </summary>
        /// <param name="point">The site position.</param>
        /// <param name="power">The non-negative site power.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="power"/> is negative or NaN.
        /// </exception>
        public Site(VectorXY point, float power)
        {
            if (power < 0f || float.IsNaN(power))
                throw new ArgumentOutOfRangeException(nameof(power), "Site power must be non-negative and not NaN.");

            Point = point;
            Power = power;
        }

        /// <summary>
        /// Gets the site position.
        /// </summary>
        public VectorXY Point { get; }

        /// <summary>
        /// Gets the site power.
        /// </summary>
        public float Power { get; }

        /// <summary>
        /// Indicates whether this site has the same point and power as another site.
        /// </summary>
        /// <param name="other">The site to compare with this site.</param>
        /// <returns><see langword="true"/> if both sites are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Site other) => Point.Equals(other.Point) && Power.Equals(other.Power);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Site other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Point, Power);

        /// <inheritdoc/>
        public override string ToString() => $"({Point}, {Power})";

        /// <summary>
        /// Indicates whether two sites are equal.
        /// </summary>
        /// <param name="left">The first site.</param>
        /// <param name="right">The second site.</param>
        /// <returns><see langword="true"/> if the sites are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Site left, Site right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two sites are different.
        /// </summary>
        /// <param name="left">The first site.</param>
        /// <param name="right">The second site.</param>
        /// <returns><see langword="true"/> if the sites are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Site left, Site right) => !left.Equals(right);
    }
}
