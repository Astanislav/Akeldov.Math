using Akeldov.Math.Spatial2D.Curves;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Represents a floating-point influence source backed by a projectable curve.
    /// </summary>
    /// <remarks>
    /// Values and powers are evaluated at the projection parameter of the sampled point. Constant
    /// constructor overloads are convenience wrappers around parameter-based providers.
    /// </remarks>
    public class FloatCurveInfluenceSource : ICurveInfluenceSource<float>
    {
        private readonly IProjectableCurve _curve;
        private readonly Func<float, float> _powerProvider;
        private readonly Func<float, float> _valueProvider;

        /// <summary>
        /// Initializes a new curve influence source with constant power and value.
        /// </summary>
        /// <param name="power">The constant source power.</param>
        /// <param name="curve">The underlying projectable curve.</param>
        /// <param name="value">The constant source value.</param>
        public FloatCurveInfluenceSource(float power, IProjectableCurve curve, float value)
        {
            if (power < 0f || float.IsNaN(power))
                throw new ArgumentOutOfRangeException(nameof(power), "Influence source power must be non-negative and not NaN.");

            if (float.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(value), "Influence source value must not be NaN.");

            _powerProvider = _ => power;
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = _ => value;
        }

        /// <summary>
        /// Initializes a new curve influence source with constant power and parameter-based values.
        /// </summary>
        /// <param name="power">The constant source power.</param>
        /// <param name="curve">The underlying projectable curve.</param>
        /// <param name="valueProvider">The value provider evaluated with the curve projection parameter.</param>
        public FloatCurveInfluenceSource(float power, IProjectableCurve curve, Func<float, float> valueProvider)
        {
            if (power < 0f || float.IsNaN(power))
                throw new ArgumentOutOfRangeException(nameof(power), "Influence source power must be non-negative and not NaN.");

            _powerProvider = _ => power;
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        /// <summary>
        /// Initializes a new curve influence source with parameter-based power and constant value.
        /// </summary>
        /// <param name="powerProvider">The power provider evaluated with the curve projection parameter.</param>
        /// <param name="curve">The underlying projectable curve.</param>
        /// <param name="value">The constant source value.</param>
        public FloatCurveInfluenceSource(Func<float, float> powerProvider, IProjectableCurve curve, float value)
        {
            if (float.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(value), "Influence source value must not be NaN.");

            _powerProvider = powerProvider ?? throw new ArgumentNullException(nameof(powerProvider));
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = _ => value;
        }

        /// <summary>
        /// Initializes a new curve influence source with parameter-based power and value.
        /// </summary>
        /// <param name="powerProvider">The power provider evaluated with the curve projection parameter.</param>
        /// <param name="curve">The underlying projectable curve.</param>
        /// <param name="valueProvider">The value provider evaluated with the curve projection parameter.</param>
        public FloatCurveInfluenceSource(Func<float, float> powerProvider, IProjectableCurve curve, Func<float, float> valueProvider)
        {
            _powerProvider = powerProvider ?? throw new ArgumentNullException(nameof(powerProvider));
            _curve = curve ?? throw new ArgumentNullException(nameof(curve));
            _valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
        }

        /// <inheritdoc/>
        public List<VectorXY> RayIntersections(Ray ray)
        {
            return _curve.RayIntersections(ray);
        }

        /// <inheritdoc/>
        public float Distance(VectorXY point)
        {
            return _curve.Distance(point);
        }

        /// <inheritdoc/>
        public CurveProjection Project(VectorXY point)
        {
            return _curve.Project(point);
        }

        /// <inheritdoc/>
        public InfluenceSample<float> GetInfluence(VectorXY point)
        {
            var projection = Project(point);
            float power = _powerProvider(projection.Parameter);
            if (power < 0f || float.IsNaN(power))
                throw new InvalidOperationException("Power provider returned an invalid influence source power. Power must be non-negative and not NaN.");

            float value = _valueProvider(projection.Parameter);
            if (float.IsNaN(value))
                throw new InvalidOperationException("Value provider returned an invalid influence source value. Value must not be NaN.");

            return new InfluenceSample<float>(
                value,
                projection.Point,
                projection.Distance,
                power);
        }
    }
}
