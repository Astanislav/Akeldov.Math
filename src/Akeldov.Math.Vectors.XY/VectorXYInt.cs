using System;

namespace Akeldov.Math.Vectors.XY
{
    public readonly struct VectorXYInt : IEquatable<VectorXYInt>
    {
        public VectorXYInt(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public float Length => MathF.Sqrt(X * X + Y * Y);

        public static VectorXYInt Zero => new VectorXYInt(0, 0);

        public static VectorXYInt One => new VectorXYInt(1, 1);

        public override bool Equals(object obj) => obj is VectorXYInt other && Equals(other);

        public bool Equals(VectorXYInt other) => X == other.X && Y == other.Y;

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"({X}, {Y})";

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public static bool operator ==(VectorXYInt left, VectorXYInt right) => left.Equals(right);

        public static bool operator !=(VectorXYInt left, VectorXYInt right) => !left.Equals(right);

        public static VectorXYInt operator +(VectorXYInt left, VectorXYInt right) =>
            new VectorXYInt(left.X + right.X, left.Y + right.Y);

        public static VectorXYInt operator -(VectorXYInt left, VectorXYInt right) =>
            new VectorXYInt(left.X - right.X, left.Y - right.Y);

        public static VectorXYInt operator *(VectorXYInt vector, int scalar) =>
            new VectorXYInt(vector.X * scalar, vector.Y * scalar);

        public static VectorXYInt operator *(int scalar, VectorXYInt vector) =>
            new VectorXYInt(vector.X * scalar, vector.Y * scalar);

        public static VectorXY operator *(VectorXYInt vector, float scalar) =>
            new VectorXY(vector.X * scalar, vector.Y * scalar);

        public static VectorXY operator *(float scalar, VectorXYInt vector) =>
            new VectorXY(vector.X * scalar, vector.Y * scalar);

        public static VectorXYInt operator /(VectorXYInt vector, int scalar)
        {
            if (scalar == 0)
                throw new DivideByZeroException("Cannot divide vector by zero.");
            return new VectorXYInt(vector.X / scalar, vector.Y / scalar);
        }
    }
}
