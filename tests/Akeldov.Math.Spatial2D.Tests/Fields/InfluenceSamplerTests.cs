using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class InfluenceSamplerTests
{
    [Test]
    public void NearestInfluenceSampler_ReturnsValueFromNearestSource()
    {
        var sources = new[]
        {
            FixedSource("far", new VectorXY(0f, 0f), distance: 5f),
            FixedSource("near", new VectorXY(10f, 0f), distance: 1f)
        };

        var sampler = new NearestInfluenceSampler<FixedInfluenceSource<string>, string>();

        string value = sampler.Sample(sources, VectorXY.Zero);

        Assert.That(value, Is.EqualTo("near"));
    }

    [Test]
    public void NearestFloatInfluenceSampler_WhenDistancesTie_ReturnsFirstValue()
    {
        var sources = new[]
        {
            FixedSource(1f, new VectorXY(0f, 0f), distance: 3f),
            FixedSource(2f, new VectorXY(10f, 0f), distance: 3f)
        };

        var sampler = new NearestFloatInfluenceSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, VectorXY.Zero);

        Assert.That(value, Is.EqualTo(1f));
    }

    [Test]
    public void NearestIntInfluenceSampler_ReturnsNearestIntegerValue()
    {
        var sources = new[]
        {
            FixedSource(10, new VectorXY(0f, 0f), distance: 4f),
            FixedSource(20, new VectorXY(10f, 0f), distance: 2f)
        };

        var sampler = new NearestIntInfluenceSampler<FixedInfluenceSource<int>>();

        int value = sampler.Sample(sources, VectorXY.Zero);

        Assert.That(value, Is.EqualTo(20));
    }

    [Test]
    public void InverseDistanceWeightedFloatSampler_UsesInverseDistanceWeights()
    {
        var sources = new[]
        {
            FixedSource(0f, new VectorXY(0f, 0f), distance: 2f),
            FixedSource(10f, new VectorXY(10f, 0f), distance: 8f)
        };

        var sampler = new InverseDistanceWeightedFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, VectorXY.Zero);

        Assert.That(value, Is.EqualTo(2f).Within(0.0001f));
    }

    [Test]
    public void InverseDistanceWeightedFloatSampler_WhenPointIsOnSource_ReturnsThatSourceValue()
    {
        var sources = new[]
        {
            FixedSource(42f, new VectorXY(0f, 0f), distance: 0f),
            FixedSource(10f, new VectorXY(10f, 0f), distance: 8f)
        };

        var sampler = new InverseDistanceWeightedFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, VectorXY.Zero);

        Assert.That(value, Is.EqualTo(42f));
    }

    [Test]
    public void InverseDistanceWeightedSampler_UsesWeightedAdditiveContract()
    {
        var sources = new[]
        {
            FixedSource(new WeightedFloat(0f), new VectorXY(0f, 0f), distance: 2f),
            FixedSource(new WeightedFloat(10f), new VectorXY(10f, 0f), distance: 8f)
        };

        var sampler = new InverseDistanceWeightedSampler<FixedInfluenceSource<WeightedFloat>, WeightedFloat>();

        WeightedFloat value = sampler.Sample(sources, VectorXY.Zero);

        Assert.That(value.Value, Is.EqualTo(2f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithSingleSource_ReturnsSourceValue()
    {
        var sources = new[]
        {
            FixedSource(7f, new VectorXY(0f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new VectorXY(100f, 100f));

        Assert.That(value, Is.EqualTo(7f));
    }

    [Test]
    public void BarycentricFloatSampler_WithTwoSources_InterpolatesAlongSegment()
    {
        var sources = new[]
        {
            FixedSource(0f, new VectorXY(0f, 0f), distance: 1f),
            FixedSource(100f, new VectorXY(10f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new VectorXY(2.5f, 0f));

        Assert.That(value, Is.EqualTo(25f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithTwoSources_ExtrapolatesOutsideSegment()
    {
        var sources = new[]
        {
            FixedSource(0f, new VectorXY(0f, 0f), distance: 1f),
            FixedSource(100f, new VectorXY(10f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new VectorXY(15f, 0f));

        Assert.That(value, Is.EqualTo(150f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithTriangle_InterpolatesInsideTriangle()
    {
        var sources = new[]
        {
            FixedSource(0f, new VectorXY(0f, 0f), distance: 1f),
            FixedSource(10f, new VectorXY(10f, 0f), distance: 1f),
            FixedSource(20f, new VectorXY(0f, 10f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new VectorXY(2f, 3f));

        Assert.That(value, Is.EqualTo(8f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithDegenerateTriangle_FallsBackToSegmentInterpolation()
    {
        var sources = new[]
        {
            FixedSource(0f, new VectorXY(0f, 0f), distance: 1f),
            FixedSource(100f, new VectorXY(10f, 0f), distance: 1f),
            FixedSource(200f, new VectorXY(20f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new VectorXY(5f, 0f));

        Assert.That(value, Is.EqualTo(50f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithMoreThanTenSources_IgnoresSamplesOutsideNearestTen()
    {
        var sources = new FixedInfluenceSource<float>[11];

        for (int i = 0; i < 10; i++)
            sources[i] = FixedSource(i * 10f, new VectorXY(i * 10f, 0f), distance: i + 1f);

        sources[10] = FixedSource(1000f, new VectorXY(0f, 10f), distance: 11f);

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new VectorXY(5f, 5f));

        Assert.That(value, Is.EqualTo(5f).Within(0.0001f));
    }

    [Test]
    public void BarycentricIntSampler_RoundsInterpolatedValue()
    {
        var sources = new[]
        {
            FixedSource(0, new VectorXY(0f, 0f), distance: 1f),
            FixedSource(10, new VectorXY(10f, 0f), distance: 1f)
        };

        var sampler = new BarycentricIntSampler<FixedInfluenceSource<int>>();

        int value = sampler.Sample(sources, new VectorXY(2.6f, 0f));

        Assert.That(value, Is.EqualTo(3));
    }

    private static FixedInfluenceSource<TValue> FixedSource<TValue>(
        TValue value,
        VectorXY sourcePoint,
        float distance,
        float power = 1f)
    {
        return new FixedInfluenceSource<TValue>(
            new InfluenceSample<TValue>(value, sourcePoint, distance, power));
    }

    private sealed class FixedInfluenceSource<TValue> : IInfluenceSource<TValue>
    {
        private readonly InfluenceSample<TValue> _influence;

        public FixedInfluenceSource(InfluenceSample<TValue> influence)
        {
            _influence = influence;
        }

        public float Distance(VectorXY point)
        {
            return _influence.Distance;
        }

        public InfluenceSample<TValue> GetInfluence(VectorXY point)
        {
            return _influence;
        }
    }

    private readonly struct WeightedFloat : IWeightedAdditive<WeightedFloat>
    {
        public WeightedFloat(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public WeightedFloat Add(WeightedFloat other)
        {
            return new WeightedFloat(Value + other.Value);
        }

        public WeightedFloat Multiply(float multiplier)
        {
            return new WeightedFloat(Value * multiplier);
        }
    }
}
