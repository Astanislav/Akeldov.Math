using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Represents a floating-point influence field sampled from curve influence sources.
    /// </summary>
    /// <remarks>
    /// Values returned by <see cref="Sample"/> are clamped to the inclusive range
    /// from <see cref="Min"/> to <see cref="Max"/>. The configured sampler may compute
    /// an extrapolated value, but this field preserves the <see cref="IFloatField"/> range contract.
    /// </remarks>
    public class CurveInfluenceFloatField : CurveInfluenceField<ICurveInfluenceSource<float>, float>, IFloatField
    {
        private readonly float _min;
        private readonly float _max;

        /// <summary>
        /// Initializes a new floating-point curve influence field.
        /// </summary>
        /// <param name="sampler">The strategy used to combine influence curves.</param>
        /// <param name="influenceSources">The influence curves used by the field.</param>
        /// <param name="min">The minimum value returned by the field.</param>
        /// <param name="max">The maximum value returned by the field.</param>
        public CurveInfluenceFloatField(
            IInfluenceSampler<ICurveInfluenceSource<float>, float> sampler,
            IReadOnlyList<ICurveInfluenceSource<float>> influenceSources,
            float min,
            float max)
            : base(sampler, influenceSources)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Initializes a new floating-point curve influence field with source culling.
        /// </summary>
        /// <param name="sampler">The strategy used to combine influence curves.</param>
        /// <param name="influenceSources">The influence curves used by the field.</param>
        /// <param name="min">The minimum value returned by the field.</param>
        /// <param name="max">The maximum value returned by the field.</param>
        /// <param name="influenceSourceCuller">The culler used to select a subset of curves for each sampled point.</param>
        public CurveInfluenceFloatField(
            IInfluenceSampler<ICurveInfluenceSource<float>, float> sampler,
            IReadOnlyList<ICurveInfluenceSource<float>> influenceSources,
            float min,
            float max,
            IInfluenceSourceCuller<ICurveInfluenceSource<float>> influenceSourceCuller)
            : base(sampler, influenceSources, influenceSourceCuller)
        {
            _min = min;
            _max = max;
        }

        /// <inheritdoc/>
        public float Min => _min;

        /// <inheritdoc/>
        public float Max => _max;

        /// <inheritdoc/>
        public override float Sample(VectorXY point)
        {
            return base.Sample(point).Clamp(Min, Max);
        }
    }
}
