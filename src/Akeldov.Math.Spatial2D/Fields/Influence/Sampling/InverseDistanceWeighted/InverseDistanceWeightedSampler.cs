using System.Collections.Generic;
using System;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Samples values using inverse-distance weighting across influence sources.
    /// </summary>
    public class InverseDistanceWeightedSampler<TSource, TValue> : IInfluenceSampler<TSource, TValue>
        where TSource : IInfluenceSource<TValue>
        where TValue : IWeightedAdditive<TValue>
    {
        public InverseDistanceWeightedSampler()
        { }

        public TValue Sample(IReadOnlyList<TSource> sources, VectorXY point)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (sources.Count == 0)
                throw new ArgumentException("Influence sources collection must not be empty.", nameof(sources));

            float totalWeight = 0f;
            TValue weightedSum = default!;
            bool hasWeightedSum = false;

            for (int i = 0; i < sources.Count; i++)
            {
                var source = sources[i];
                var influence = source.GetInfluence(point);
                if (influence.Distance <= GeometryConstants.GeometryEpsilon)
                    return influence.Value;

                var weight = influence.Power / influence.Distance;
                totalWeight += weight;

                var weightedTerm = influence.Value.Multiply(weight);
                weightedSum = hasWeightedSum
                    ? weightedSum.Add(weightedTerm)
                    : weightedTerm;
                hasWeightedSum = true;
            }

            return totalWeight > 0f && hasWeightedSum
                ? weightedSum.Multiply(1f / totalWeight)
                : sources[0].GetInfluence(point).Value;
        }
    }
}
