using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    public class PointInfluenceField<TPointSource, TValue> : InfluenceField<TPointSource, TValue>
        where TPointSource : IPointInfluenceSource<TValue>
    {
        public PointInfluenceField(
            IInfluenceSampler<TPointSource, TValue> sampler,
            IReadOnlyList<TPointSource> influenceSources)
            : base(sampler, influenceSources)
        {
        }

        public PointInfluenceField(
            IInfluenceSampler<TPointSource, TValue> sampler,
            IReadOnlyList<TPointSource> influenceSources,
            IInfluenceSourceCuller<TPointSource> influenceSourceCuller)
            : base(sampler, influenceSources, influenceSourceCuller)
        {
        }

        public IReadOnlyList<TPointSource> InfluencePoints => InfluenceSources;
    }
}
