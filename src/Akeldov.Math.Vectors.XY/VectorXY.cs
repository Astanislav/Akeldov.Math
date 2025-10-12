using System;

namespace Akeldov.Math.Vectors.XY
{
    public readonly struct VectorXY : IEquatable<VectorXY>
    {
        public VectorXY(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }

        public float Y { get; }

        public float Length => MathF.Sqrt(X * X + Y * Y);

        public float SQRLength => X * X + Y * Y;

        public static VectorXY Zero => new VectorXY(0f, 0f);

        public static VectorXY One => new VectorXY(1f, 1f);

        public VectorXY Normalize()
        {
            float length = Length;
            return length == 0f ? Zero : new VectorXY(X / length, Y / length);
        }

        public VectorXYInt RoundToInt()
        {
            return new VectorXYInt((int)MathF.Round(X), (int)MathF.Round(Y));
        }

        public bool Equals(VectorXY other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object obj) => obj is VectorXY other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"({X}, {Y})";

        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }

        public static float Dot(in VectorXY left, in VectorXY right) => left.X * right.X + left.Y * right.Y;

        public static float Cross(in VectorXY left, in VectorXY right) => left.X * right.Y - left.Y * right.X;

        public static float Angle(VectorXY from, VectorXY to)
        {
            float dot = Dot(from, to);
            float cross = Cross(from, to);

            return MathF.Atan2(cross, dot);
        }

        public static bool operator ==(VectorXY left, VectorXY right) => left.Equals(right);

        public static bool operator !=(VectorXY left, VectorXY right) => !left.Equals(right);

        public static VectorXY operator +(VectorXY left, VectorXY right) =>
            new VectorXY(left.X + right.X, left.Y + right.Y);

        public static VectorXY operator -(VectorXY left, VectorXY right) =>
            new VectorXY(left.X - right.X, left.Y - right.Y);

        public static VectorXY operator *(VectorXY vector, float scalar) =>
            new VectorXY(vector.X * scalar, vector.Y * scalar);

        public static VectorXY operator *(float scalar, VectorXY vector) =>
            new VectorXY(vector.X * scalar, vector.Y * scalar);

        public static VectorXY operator *(VectorXY vector, int scalar) =>
            new VectorXY(vector.X * scalar, vector.Y * scalar);

        public static VectorXY operator *(int scalar, VectorXY vector) =>
            new VectorXY(vector.X * scalar, vector.Y * scalar);

        public static VectorXY operator /(VectorXY vector, float scalar) =>
            new VectorXY(vector.X / scalar, vector.Y / scalar);

        public static implicit operator VectorXY(VectorXYInt v)
        {
            return new VectorXY(v.X, v.Y);
        }

        public static explicit operator VectorXYInt(VectorXY v)
        {
            return new VectorXYInt((int)v.X, (int)v.Y);
        }
    }
}
