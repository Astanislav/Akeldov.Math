using System.Collections.Generic;
using System;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Samples floating-point values using inverse-distance weighting across influence sources.
    /// </summary>
    public class InverseDistanceWeightedFloatSampler<TSource> : IInfluenceSampler<TSource, float>
        where TSource : IInfluenceSource<float>
    {
        public InverseDistanceWeightedFloatSampler()
        { }

        public float Sample(IReadOnlyList<TSource> sources, VectorXY point)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (sources.Count == 0)
                throw new ArgumentException("Influence sources collection must not be empty.", nameof(sources));

            float totalWeight = 0f;
            float weightedSum = 0f;

            for (int i = 0; i < sources.Count; i++)
            {
                var source = sources[i];
                var influence = source.GetInfluence(point);
                if (influence.Power <= 0f || float.IsNaN(influence.Power) || float.IsInfinity(influence.Power))
                    throw new InvalidOperationException("Inverse-distance weighted sampler requires finite positive source power.");

                if (influence.Distance <= GeometryConstants.GeometryEpsilon)
                    return influence.Value;

                float weight = influence.Power / influence.Distance;
                totalWeight += weight;
                weightedSum += influence.Value * weight;
            }

            return totalWeight > 0f ? (weightedSum / totalWeight) : 0f;
        }
    }
}
