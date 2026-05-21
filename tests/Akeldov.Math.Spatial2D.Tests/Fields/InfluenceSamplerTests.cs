using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class InfluenceSamplerTests
{
    [Test]
    public void NearestInfluenceSampler_ReturnsValueFromNearestSource()
    {
        var sources = new[]
        {
            FixedSource("far", new PointXY(0f, 0f), distance: 5f),
            FixedSource("near", new PointXY(10f, 0f), distance: 1f)
        };

        var sampler = new NearestInfluenceSampler<FixedInfluenceSource<string>, string>();

        string value = sampler.Sample(sources, new PointXY(0f, 0f));

        Assert.That(value, Is.EqualTo("near"));
    }

    [Test]
    public void NearestFloatInfluenceSampler_WhenDistancesTie_ReturnsFirstValue()
    {
        var sources = new[]
        {
            FixedSource(1f, new PointXY(0f, 0f), distance: 3f),
            FixedSource(2f, new PointXY(10f, 0f), distance: 3f)
        };

        var sampler = new NearestFloatInfluenceSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(0f, 0f));

        Assert.That(value, Is.EqualTo(1f));
    }

    [Test]
    public void NearestIntInfluenceSampler_ReturnsNearestIntegerValue()
    {
        var sources = new[]
        {
            FixedSource(10, new PointXY(0f, 0f), distance: 4f),
            FixedSource(20, new PointXY(10f, 0f), distance: 2f)
        };

        var sampler = new NearestIntInfluenceSampler<FixedInfluenceSource<int>>();

        int value = sampler.Sample(sources, new PointXY(0f, 0f));

        Assert.That(value, Is.EqualTo(20));
    }

    [Test]
    public void NearestInfluenceSampler_WhenSourcesContainNull_ThrowsArgumentException()
    {
        var sources = new FixedInfluenceSource<string>[]
        {
            FixedSource("valid", new PointXY(0f, 0f), distance: 1f),
            null!
        };
        var sampler = new NearestInfluenceSampler<FixedInfluenceSource<string>, string>();

        AssertThrowsForNullSource(() =>
            sampler.Sample(sources, new PointXY(0f, 0f)));
    }

    [Test]
    public void NearestFloatInfluenceSampler_WhenSourcesContainNull_ThrowsArgumentException()
    {
        var sources = new FixedInfluenceSource<float>[]
        {
            FixedSource(1f, new PointXY(0f, 0f), distance: 1f),
            null!
        };
        var sampler = new NearestFloatInfluenceSampler<FixedInfluenceSource<float>>();

        AssertThrowsForNullSource(() =>
            sampler.Sample(sources, new PointXY(0f, 0f)));
    }

    [Test]
    public void NearestIntInfluenceSampler_WhenSourcesContainNull_ThrowsArgumentException()
    {
        var sources = new FixedInfluenceSource<int>[]
        {
            FixedSource(1, new PointXY(0f, 0f), distance: 1f),
            null!
        };
        var sampler = new NearestIntInfluenceSampler<FixedInfluenceSource<int>>();

        AssertThrowsForNullSource(() =>
            sampler.Sample(sources, new PointXY(0f, 0f)));
    }

    [Test]
    public void InverseDistanceWeightedFloatSampler_UsesInverseDistanceWeights()
    {
        var sources = new[]
        {
            FixedSource(0f, new PointXY(0f, 0f), distance: 2f),
            FixedSource(10f, new PointXY(10f, 0f), distance: 8f)
        };

        var sampler = new InverseDistanceWeightedFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(0f, 0f));

        Assert.That(value, Is.EqualTo(2f).Within(0.0001f));
    }

    [Test]
    public void InverseDistanceWeightedFloatSampler_WhenPointIsOnSource_ReturnsThatSourceValue()
    {
        var sources = new[]
        {
            FixedSource(42f, new PointXY(0f, 0f), distance: 0f),
            FixedSource(10f, new PointXY(10f, 0f), distance: 8f)
        };

        var sampler = new InverseDistanceWeightedFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(0f, 0f));

        Assert.That(value, Is.EqualTo(42f));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void InverseDistanceWeightedFloatSampler_WhenWeightIsUnsupported_Throws(float weight)
    {
        var sources = new[]
        {
            FixedSource(0f, new PointXY(0f, 0f), distance: 2f, weight: weight),
            FixedSource(10f, new PointXY(10f, 0f), distance: 8f)
        };

        var sampler = new InverseDistanceWeightedFloatSampler<FixedInfluenceSource<float>>();

        Assert.Throws<InvalidOperationException>(() =>
            sampler.Sample(sources, new PointXY(0f, 0f)));
    }

    [Test]
    public void InverseDistanceWeightedFloatSampler_WhenSourcesContainNull_ThrowsArgumentException()
    {
        var sources = new FixedInfluenceSource<float>[]
        {
            FixedSource(1f, new PointXY(0f, 0f), distance: 1f),
            null!
        };
        var sampler = new InverseDistanceWeightedFloatSampler<FixedInfluenceSource<float>>();

        AssertThrowsForNullSource(() =>
            sampler.Sample(sources, new PointXY(0f, 0f)));
    }

    [Test]
    public void BarycentricFloatSampler_WithSingleSource_ReturnsSourceValue()
    {
        var sources = new[]
        {
            FixedSource(7f, new PointXY(0f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(100f, 100f));

        Assert.That(value, Is.EqualTo(7f));
    }

    [Test]
    public void BarycentricFloatSampler_WithTwoSources_InterpolatesAlongSegment()
    {
        var sources = new[]
        {
            FixedSource(0f, new PointXY(0f, 0f), distance: 1f),
            FixedSource(100f, new PointXY(10f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(2.5f, 0f));

        Assert.That(value, Is.EqualTo(25f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithTwoSources_ExtrapolatesOutsideSegment()
    {
        var sources = new[]
        {
            FixedSource(0f, new PointXY(0f, 0f), distance: 1f),
            FixedSource(100f, new PointXY(10f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(15f, 0f));

        Assert.That(value, Is.EqualTo(150f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithTriangle_InterpolatesInsideTriangle()
    {
        var sources = new[]
        {
            FixedSource(0f, new PointXY(0f, 0f), distance: 1f),
            FixedSource(10f, new PointXY(10f, 0f), distance: 1f),
            FixedSource(20f, new PointXY(0f, 10f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(2f, 3f));

        Assert.That(value, Is.EqualTo(8f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithDegenerateTriangle_FallsBackToSegmentInterpolation()
    {
        var sources = new[]
        {
            FixedSource(0f, new PointXY(0f, 0f), distance: 1f),
            FixedSource(100f, new PointXY(10f, 0f), distance: 1f),
            FixedSource(200f, new PointXY(20f, 0f), distance: 1f)
        };

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(5f, 0f));

        Assert.That(value, Is.EqualTo(50f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithDefaultCandidateLimit_IgnoresSamplesOutsideNearestTen()
    {
        var sources = new FixedInfluenceSource<float>[11];

        for (int i = 0; i < 10; i++)
            sources[i] = FixedSource(i * 10f, new PointXY(i * 10f, 0f), distance: i + 1f);

        sources[10] = FixedSource(1000f, new PointXY(0f, 10f), distance: 11f);

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        float value = sampler.Sample(sources, new PointXY(5f, 5f));

        Assert.That(value, Is.EqualTo(5f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WithCustomCandidateLimit_ConsidersAdditionalSamples()
    {
        var sources = new FixedInfluenceSource<float>[11];

        for (int i = 0; i < 10; i++)
            sources[i] = FixedSource(i * 10f, new PointXY(i * 10f, 0f), distance: i + 1f);

        sources[10] = FixedSource(1000f, new PointXY(0f, 10f), distance: 11f);

        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>(11);

        float value = sampler.Sample(sources, new PointXY(5f, 5f));

        Assert.That(value, Is.EqualTo(505f).Within(0.0001f));
    }

    [Test]
    public void BarycentricFloatSampler_WhenCandidateLimitIsLessThanThree_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BarycentricFloatSampler<FixedInfluenceSource<float>>(2));
    }

    [Test]
    public void BarycentricFloatSampler_WhenSourcesContainNull_ThrowsArgumentException()
    {
        var sources = new FixedInfluenceSource<float>[]
        {
            FixedSource(1f, new PointXY(0f, 0f), distance: 1f),
            null!
        };
        var sampler = new BarycentricFloatSampler<FixedInfluenceSource<float>>();

        AssertThrowsForNullSource(() =>
            sampler.Sample(sources, new PointXY(0f, 0f)));
    }

    [Test]
    public void BarycentricIntSampler_RoundsInterpolatedValue()
    {
        var sources = new[]
        {
            FixedSource(0, new PointXY(0f, 0f), distance: 1f),
            FixedSource(10, new PointXY(10f, 0f), distance: 1f)
        };

        var sampler = new BarycentricIntSampler<FixedInfluenceSource<int>>();

        int value = sampler.Sample(sources, new PointXY(2.6f, 0f));

        Assert.That(value, Is.EqualTo(3));
    }

    [Test]
    public void BarycentricIntSampler_WithCustomCandidateLimit_ConsidersAdditionalSamples()
    {
        var sources = new FixedInfluenceSource<int>[11];

        for (int i = 0; i < 10; i++)
            sources[i] = FixedSource(i * 10, new PointXY(i * 10f, 0f), distance: i + 1f);

        sources[10] = FixedSource(1000, new PointXY(0f, 10f), distance: 11f);

        var sampler = new BarycentricIntSampler<FixedInfluenceSource<int>>(11);

        int value = sampler.Sample(sources, new PointXY(5f, 5f));

        Assert.That(value, Is.EqualTo(505));
    }

    [Test]
    public void BarycentricIntSampler_WhenCandidateLimitIsLessThanThree_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BarycentricIntSampler<FixedInfluenceSource<int>>(2));
    }

    [Test]
    public void BarycentricIntSampler_WhenSourcesContainNull_ThrowsArgumentException()
    {
        var sources = new FixedInfluenceSource<int>[]
        {
            FixedSource(1, new PointXY(0f, 0f), distance: 1f),
            null!
        };
        var sampler = new BarycentricIntSampler<FixedInfluenceSource<int>>();

        AssertThrowsForNullSource(() =>
            sampler.Sample(sources, new PointXY(0f, 0f)));
    }

    [Test]
    public void BuiltInSamplers_WhenPointCoordinateIsInvalid_ThrowArgumentOutOfRangeException()
    {
        var stringSources = new[] { FixedSource("value", new PointXY(0f, 0f), distance: 1f) };
        var floatSources = new[] { FixedSource(1f, new PointXY(0f, 0f), distance: 1f) };
        var intSources = new[] { FixedSource(1, new PointXY(0f, 0f), distance: 1f) };
        var invalidPoint = new PointXY(float.PositiveInfinity, 0f);

        AssertThrowsForInvalidPoint(() =>
            new NearestInfluenceSampler<FixedInfluenceSource<string>, string>().Sample(stringSources, invalidPoint));
        AssertThrowsForInvalidPoint(() =>
            new NearestFloatInfluenceSampler<FixedInfluenceSource<float>>().Sample(floatSources, invalidPoint));
        AssertThrowsForInvalidPoint(() =>
            new NearestIntInfluenceSampler<FixedInfluenceSource<int>>().Sample(intSources, invalidPoint));
        AssertThrowsForInvalidPoint(() =>
            new InverseDistanceWeightedFloatSampler<FixedInfluenceSource<float>>().Sample(floatSources, invalidPoint));
        AssertThrowsForInvalidPoint(() =>
            new BarycentricFloatSampler<FixedInfluenceSource<float>>().Sample(floatSources, invalidPoint));
        AssertThrowsForInvalidPoint(() =>
            new BarycentricIntSampler<FixedInfluenceSource<int>>().Sample(intSources, invalidPoint));
    }

    private static void AssertThrowsForNullSource(TestDelegate action)
    {
        var exception = Assert.Throws<ArgumentException>(action);

        Assert.That(exception!.ParamName, Is.EqualTo("sources"));
    }

    private static void AssertThrowsForInvalidPoint(TestDelegate action)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(action);

        Assert.That(exception!.ParamName, Is.EqualTo("point"));
    }

    private static FixedInfluenceSource<TValue> FixedSource<TValue>(
        TValue value,
        PointXY sourcePoint,
        float distance,
        float weight = 1f)
    {
        return new FixedInfluenceSource<TValue>(
            new InfluenceSample<TValue>(value, sourcePoint, distance, weight));
    }

    private sealed class FixedInfluenceSource<TValue> : IInfluenceSource<TValue>
    {
        private readonly InfluenceSample<TValue> _influence;

        public FixedInfluenceSource(InfluenceSample<TValue> influence)
        {
            _influence = influence;
        }

        public float Distance(PointXY point)
        {
            return _influence.Distance;
        }

        public InfluenceSample<TValue> GetInfluence(PointXY point)
        {
            return _influence;
        }
    }
}
