using System.Collections.Generic;
using System;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Samples the integer value contributed by the nearest influence source.
    /// </summary>
    public class NearestIntInfluenceSampler<TSource> : IInfluenceSampler<TSource, int>
        where TSource : IInfluenceSource<int>
    {
        public NearestIntInfluenceSampler()
        { }

        public int Sample(IReadOnlyList<TSource> sources, VectorXY point)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            if (sources.Count == 0)
                throw new ArgumentException("Influence sources collection must not be empty.", nameof(sources));

            var nearestInfluence = sources[0].GetInfluence(point);

            for (var i = 1; i < sources.Count; i++)
            {
                var candidateInfluence = sources[i].GetInfluence(point);

                if (candidateInfluence.Distance < nearestInfluence.Distance)
                    nearestInfluence = candidateInfluence;
            }

            return nearestInfluence.Value;
        }
    }
}
