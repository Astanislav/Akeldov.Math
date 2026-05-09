using System;
using System.Runtime.InteropServices;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Site : IEquatable<Site>
    {
        public readonly VectorXY Point;

        public readonly float Power;

        public Site(VectorXY point, float power)
        {
            Point = point;
            Power = power;
        }

        public bool Equals(Site other) => Point.Equals(other.Point) && Power.Equals(other.Power);

        public override bool Equals(object? obj) => obj is Site other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Point, Power);

        public override string ToString() => $"({Point}, {Power})";

        public static bool operator ==(Site left, Site right) => left.Equals(right);

        public static bool operator !=(Site left, Site right) => !left.Equals(right);
    }
}
