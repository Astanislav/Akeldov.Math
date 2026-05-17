using System;
using System.Collections.Generic;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Contours
{
    /// <summary>
    /// Represents a filled two-dimensional region defined by one or more closed contours.
    /// </summary>
    public sealed class Region : IRegion
    {
        private readonly IContour[] _contours;
        private readonly IReadOnlyList<IContour> _readOnlyContours;
        private readonly IBoundedParameterizedCurve[] _boundaryCurves;

        /// <summary>
        /// Initializes a new region from the specified contours and fill rule.
        /// </summary>
        /// <param name="contours">The contours that define the region.</param>
        /// <param name="fillRule">The fill rule used to interpret the contours.</param>
        public Region(IReadOnlyList<IContour> contours, FillRule fillRule = FillRule.EvenOdd)
        {
            if (contours == null)
                throw new ArgumentNullException(nameof(contours));

            if (contours.Count == 0)
                throw new ArgumentException("A region must contain at least one contour.", nameof(contours));

            if (fillRule != FillRule.EvenOdd)
                throw new ArgumentOutOfRangeException(nameof(fillRule), "The fill rule is not supported.");

            _contours = new IContour[contours.Count];
            var boundaryCurves = new List<IBoundedParameterizedCurve>();

            for (int i = 0; i < contours.Count; i++)
            {
                IContour contour = contours[i] ?? throw new ArgumentException("A region cannot contain null contours.", nameof(contours));
                IReadOnlyList<IBoundedParameterizedCurve> curves = contour.Curves;

                if (curves == null || curves.Count == 0)
                    throw new ArgumentException("Region contours must expose at least one bounded parameterized curve.", nameof(contours));

                for (int j = 0; j < curves.Count; j++)
                {
                    IBoundedParameterizedCurve curve = curves[j] ?? throw new ArgumentException("Region contour curves must not contain null values.", nameof(contours));
                    boundaryCurves.Add(curve);
                }

                _contours[i] = contour;
            }

            FillRule = fillRule;
            _readOnlyContours = Array.AsReadOnly(_contours);
            _boundaryCurves = boundaryCurves.ToArray();
        }

        /// <inheritdoc/>
        public IReadOnlyList<IContour> Contours => _readOnlyContours;

        /// <inheritdoc/>
        public FillRule FillRule { get; }

        /// <inheritdoc/>
        public bool Contains(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            if (IsOnBoundary(point))
                return true;

            int containingContours = 0;

            for (int i = 0; i < _contours.Length; i++)
            {
                if (_contours[i].Contains(point))
                    containingContours++;
            }

            return containingContours % 2 == 1;
        }

        private bool IsOnBoundary(VectorXY point)
        {
            for (int i = 0; i < _boundaryCurves.Length; i++)
            {
                if (_boundaryCurves[i].Distance(point) <= GeometryConstants.GeometryEpsilon)
                    return true;
            }

            return false;
        }
    }
}
