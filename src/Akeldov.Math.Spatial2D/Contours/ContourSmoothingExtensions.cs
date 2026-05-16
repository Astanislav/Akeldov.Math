using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Contours
{
    /// <summary>
    /// Provides contour smoothing helpers.
    /// </summary>
    public static class ContourSmoothingExtensions
    {
        /// <summary>
        /// Returns a contour with tangent arcs inserted at corners where two adjacent segments meet.
        /// </summary>
        /// <param name="contour">The contour to smooth.</param>
        /// <param name="radius">The radius of inserted tangent arcs.</param>
        /// <returns>A new contour with segment-segment corners smoothed by arcs.</returns>
        public static Contour SmoothRadii(this IContour contour, float radius)
        {
            if (contour == null)
                throw new ArgumentNullException(nameof(contour));

            if (radius <= 0f || float.IsNaN(radius) || float.IsInfinity(radius))
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be finite and positive.");

            IReadOnlyList<IBoundedParameterizedCurve> curves = contour.Curves;
            if (curves == null || curves.Count == 0)
                throw new InvalidOperationException("Contour must expose at least one bounded parameterized curve.");

            Arc?[] cornerArcs = CreateCornerArcs(curves, radius);
            var smoothedCurves = new List<IBoundedParameterizedCurve>(curves.Count + cornerArcs.Length);

            for (int i = 0; i < curves.Count; i++)
            {
                IBoundedParameterizedCurve curve = curves[i];

                if (curve is Segment segment)
                {
                    Arc? startArc = cornerArcs[GetPreviousIndex(i, curves.Count)];
                    Arc? endArc = cornerArcs[i];
                    smoothedCurves.Add(CreateTrimmedSegment(segment, startArc, endArc));
                }
                else
                {
                    smoothedCurves.Add(curve);
                }

                Arc? cornerArc = cornerArcs[i];
                if (cornerArc.HasValue)
                    smoothedCurves.Add(cornerArc.Value);
            }

            return new Contour(smoothedCurves);
        }

        private static Arc?[] CreateCornerArcs(IReadOnlyList<IBoundedParameterizedCurve> curves, float radius)
        {
            var cornerArcs = new Arc?[curves.Count];

            for (int i = 0; i < curves.Count; i++)
            {
                int nextIndex = (i + 1) % curves.Count;

                if (curves[i] is Segment previousSegment &&
                    curves[nextIndex] is Segment nextSegment &&
                    previousSegment.EndPoint.AlmostEquals(nextSegment.StartPoint) &&
                    TryCreateFilletArc(previousSegment, nextSegment, radius, out Arc arc))
                {
                    cornerArcs[i] = arc;
                }
            }

            return cornerArcs;
        }

        private static bool TryCreateFilletArc(Segment previousSegment, Segment nextSegment, float radius, out Arc arc)
        {
            try
            {
                arc = CornerExtensions.CreateFilletArc(
                    previousSegment.StartPoint,
                    previousSegment.EndPoint,
                    nextSegment.EndPoint,
                    radius);
                return true;
            }
            catch (ArgumentException)
            {
                arc = default;
                return false;
            }
        }

        private static Segment CreateTrimmedSegment(Segment segment, Arc? startArc, Arc? endArc)
        {
            VectorXY startPoint = segment.StartPoint;
            VectorXY endPoint = segment.EndPoint;
            bool includesStartPoint = segment.IncludesStartPoint;
            bool includesEndPoint = segment.IncludesEndPoint;

            if (startArc.HasValue)
            {
                startPoint = GetArcTangentPointOnSegmentLine(startArc.Value, segment, segment.StartPoint);
                includesStartPoint = false;
            }

            if (endArc.HasValue)
            {
                endPoint = GetArcTangentPointOnSegmentLine(endArc.Value, segment, segment.EndPoint);
                includesEndPoint = false;
            }

            return new Segment(startPoint, endPoint, includesStartPoint, includesEndPoint);
        }

        private static VectorXY GetArcTangentPointOnSegmentLine(Arc arc, Segment segment, VectorXY fallbackPoint)
        {
            if (segment.Length <= GeometryConstants.GeometryEpsilon)
                return fallbackPoint;

            var line = new Line(segment.StartPoint, segment.EndPoint);
            float startDistance = line.Distance(arc.StartPoint);
            float endDistance = line.Distance(arc.EndPoint);

            if (startDistance < endDistance - GeometryConstants.GeometryEpsilon)
                return arc.StartPoint;

            if (endDistance < startDistance - GeometryConstants.GeometryEpsilon)
                return arc.EndPoint;

            return fallbackPoint.Distance(arc.StartPoint) <= fallbackPoint.Distance(arc.EndPoint)
                ? arc.StartPoint
                : arc.EndPoint;
        }

        private static int GetPreviousIndex(int index, int count)
        {
            return index == 0 ? count - 1 : index - 1;
        }
    }
}
