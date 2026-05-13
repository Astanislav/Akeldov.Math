using Akeldov.Math.Spatial2D;
using System;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Provides helper methods for <see cref="Segment"/>.
    /// </summary>
    public static class SegmentExtensions
    {
        /// <summary>
        /// Shortens a segment by moving each endpoint toward the other endpoint.
        /// </summary>
        /// <param name="segment">The segment to shorten.</param>
        /// <param name="amount">The amount removed from each end.</param>
        /// <returns>The shortened segment.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is negative, NaN, or infinite.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the segment is too short.</exception>
        public static Segment Shorten(this Segment segment, float amount)
        {
            if (amount < 0f || float.IsNaN(amount) || float.IsInfinity(amount))
                throw new ArgumentOutOfRangeException(nameof(amount), "Segment extension amount must be finite and non-negative.");

            float length = segment.StartPoint.Distance(segment.EndPoint);

            if (2 * amount >= length)
                throw new InvalidOperationException("The segment is too short to shorten by the requested amount.");

            VectorXY direction = (segment.EndPoint - segment.StartPoint).Normalize();

            var startPoint = segment.StartPoint + direction * amount;
            var endPoint = segment.EndPoint - direction * amount;

            return new Segment(startPoint, endPoint, segment.IncludesStartPoint, segment.IncludesEndPoint);
        }

        /// <summary>
        /// Extends a segment by moving each endpoint away from the other endpoint.
        /// </summary>
        /// <param name="segment">The segment to extend.</param>
        /// <param name="amount">The amount added to each end.</param>
        /// <returns>The extended segment.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="amount"/> is negative, NaN, or infinite.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the segment has equal endpoints.</exception>
        public static Segment Extend(this Segment segment, float amount)
        {
            if (amount < 0f || float.IsNaN(amount) || float.IsInfinity(amount))
                throw new ArgumentOutOfRangeException(nameof(amount), "Segment extension amount must be finite and non-negative.");

            VectorXY direction = (segment.EndPoint - segment.StartPoint).Normalize();

            if (direction == VectorXY.Zero)
                throw new InvalidOperationException("Cannot extend a segment with equal endpoints.");

            var startPoint = segment.StartPoint - direction * amount;
            var endPoint = segment.EndPoint + direction * amount;

            return new Segment(startPoint, endPoint, segment.IncludesStartPoint, segment.IncludesEndPoint);
        }
    }
}
