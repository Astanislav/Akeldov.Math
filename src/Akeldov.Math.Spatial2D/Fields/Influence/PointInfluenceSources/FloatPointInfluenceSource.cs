namespace Akeldov.Math.Spatial2D.Fields
{
    public class FloatPointInfluenceSource : IPointInfluenceSource<float>
    {
        public FloatPointInfluenceSource(float power, VectorXY center, float value)
        {
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
