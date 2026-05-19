using Akeldov.Math.Spatial2D.Curves;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Represents a floating-point influence source backed by a parameterized projectable curve.
    /// </summary>
    /// <remarks>
    /// Values and weights are evaluated at the curve coordinate of the sampled point. Constant
    /// constructor overloads are convenience wrappers around coordinate-based providers.
    /// </remarks>
    public class FloatCurveInfluenceSource : ICurveInfluenceSource<float>
    {
        private readonly IParameterizedProjectableCurve _curve;
        private readonly Func<float, float> _weightProvider;
        private readonly Func<float, float> _valueProvider;

        /// <summary>
        /// Initializes a new curve influence source with constant weight and value.
        /// </summary>
        /// <param name="weight">The constant source weight.</param>
        /// <param name="curve">The underlying parameterized projectable curve.</param>
        /// <param name="value">The constant source value.</param>
        public FloatCurveInfluenceSource(float weight, IParameterizedProjectableCurve curve, float value)
        {
            if (weight < 0f || float.IsNaN(weight))
                throw new ArgumentOutOfRangeException(nameof(weight), "Influence source weight must be non-negative and not NaN.");

            if (float.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(value), "Influence source value must not be NaN.");

            _weightProvider = _ => weight;
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = _ => value;
        }

        /// <summary>
        /// Initializes a new curve influence source with constant weight and coordinate-based values.
        /// </summary>
        /// <param name="weight">The constant source weight.</param>
        /// <param name="curve">The underlying parameterized projectable curve.</param>
        /// <param name="valueProvider">The value provider evaluated with the curve coordinate.</param>
        public FloatCurveInfluenceSource(float weight, IParameterizedProjectableCurve curve, Func<float, float> valueProvider)
        {
            if (weight < 0f || float.IsNaN(weight))
                throw new ArgumentOutOfRangeException(nameof(weight), "Influence source weight must be non-negative and not NaN.");

            _weightProvider = _ => weight;
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        /// <summary>
        /// Initializes a new curve influence source with coordinate-based weight and constant value.
        /// </summary>
        /// <param name="weightProvider">The weight provider evaluated with the curve coordinate.</param>
        /// <param name="curve">The underlying parameterized projectable curve.</param>
        /// <param name="value">The constant source value.</param>
        public FloatCurveInfluenceSource(Func<float, float> weightProvider, IParameterizedProjectableCurve curve, float value)
        {
            if (float.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(value), "Influence source value must not be NaN.");

            _weightProvider = weightProvider ?? throw new ArgumentNullException(nameof(weightProvider));
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = _ => value;
        }

        /// <summary>
        /// Initializes a new curve influence source with coordinate-based weight and value.
        /// </summary>
        /// <param name="weightProvider">The weight provider evaluated with the curve coordinate.</param>
        /// <param name="curve">The underlying parameterized projectable curve.</param>
        /// <param name="valueProvider">The value provider evaluated with the curve coordinate.</param>
        public FloatCurveInfluenceSource(Func<float, float> weightProvider, IParameterizedProjectableCurve curve, Func<float, float> valueProvider)
        {
            _weightProvider = weightProvider ?? throw new ArgumentNullException(nameof(weightProvider));
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        /// <inheritdoc/>
        public List<VectorXY> GetRayIntersections(
            Ray ray,
            float geometryEpsilon = GeometryConstants.GeometryEpsilon)
        {
            GeometryConstants.ValidateGeometryEpsilon(geometryEpsilon, nameof(geometryEpsilon));

            return _curve.GetRayIntersections(ray, geometryEpsilon);
        }

        /// <inheritdoc/>
        public float Distance(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            return _curve.Distance(point);
        }

        /// <inheritdoc/>
        public CurveProjection Project(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            return _curve.Project(point);
        }

        /// <inheritdoc/>
        public ParameterizedCurveProjection ProjectWithParameter(VectorXY point)
        {
            if (!point.IsFinite)
                throw new ArgumentOutOfRangeException(nameof(point), "Point coordinates must be finite.");

            return _curve.ProjectWithParameter(point);
        }

        /// <inheritdoc/>
        public InfluenceSample<float> GetInfluence(VectorXY point)
        {
            var projection = ProjectWithParameter(point);
            float weight = _weightProvider(projection.CurveCoordinate);
            if (weight < 0f || float.IsNaN(weight))
                throw new InvalidOperationException("Weight provider returned an invalid influence source weight. Weight must be non-negative and not NaN.");

            float value = _valueProvider(projection.CurveCoordinate);
            if (float.IsNaN(value))
                throw new InvalidOperationException("Value provider returned an invalid influence source value. Value must not be NaN.");

            return new InfluenceSample<float>(
                value,
                projection.ProjectedPoint,
                projection.Distance,
                weight);
        }
    }
}
