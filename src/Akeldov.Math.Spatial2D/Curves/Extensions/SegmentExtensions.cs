using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Spatial2D.Curves
{
    public static class SegmentExtensions
    {
        public static Segment Shorten(this Segment segment, float amount)
        {
            if (amount < 0f)
                throw new ArgumentOutOfRangeException(nameof(amount));

            float length = segment.A.Distance(segment.B);

            if (2 * amount >= length)
                throw new InvalidOperationException("The segment is too short to shorten by the requested amount.");

            VectorXY direction = (segment.B - segment.A).Normalize();

            var a = segment.A + direction * amount;
            var b = segment.B - direction * amount;

            return new Segment(a, b);
        }

        public static Segment Extend(this Segment segment, float amount)
        {
            if (amount < 0f)
                throw new ArgumentOutOfRangeException(nameof(amount));

            VectorXY direction = (segment.B - segment.A).Normalize();

            if (direction == VectorXY.Zero)
                throw new InvalidOperationException("Cannot extend a segment with equal endpoints.");

            var a = segment.A - direction * amount;
            var b = segment.B + direction * amount;

            return new Segment(a, b);
        }
    }
}
