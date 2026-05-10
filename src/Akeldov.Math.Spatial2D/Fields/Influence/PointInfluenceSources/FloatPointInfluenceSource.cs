using System;

namespace Akeldov.Math.Spatial2D.Fields
{
    public class FloatPointInfluenceSource : IPointInfluenceSource<float>
    {
        public FloatPointInfluenceSource(float power, VectorXY center, float value)
        {
            if (power < 0f || float.IsNaN(power))
                throw new ArgumentOutOfRangeException(nameof(power), "Influence source power must be non-negative and not NaN.");

            Power = power;
            Center = center;
            Value = value;
        }

        public float Power { get; }

        public VectorXY Center { get; }

        public float Value { get; }

        public float Distance(VectorXY point)
        {
            return Center.Distance(point);
        }

        public InfluenceSample<float> GetInfluence(VectorXY point)
        {
            return new InfluenceSample<float>(Value, Center, Distance(point), Power);
        }
    }
}
