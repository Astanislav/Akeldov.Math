using System;

namespace Akeldov.Math.Spatial2D.Fields
{
    public class IntPointInfluenceSource : IPointInfluenceSource<int>
    {
        public IntPointInfluenceSource(float power, VectorXY center, int value)
        {
            if (power < 0f || float.IsNaN(power))
                throw new ArgumentOutOfRangeException(nameof(power), "Influence source power must be non-negative and not NaN.");

            Power = power;
            Center = center;
            Value = value;
        }

        public float Power { get; }

        public VectorXY Center { get; }

        public int Value { get; }

        public float Distance(VectorXY point)
        {
            return Center.Distance(point);
        }

        public InfluenceSample<int> GetInfluence(VectorXY point)
        {
            return new InfluenceSample<int>(Value, Center, Distance(point), Power);
        }
    }
}
