using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class InfluenceFieldCullingTests
{
    [Test]
    public void Sample_WhenCullerReturnsSubset_UsesSelectedSources()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, VectorXY.Zero, 10f),
            new FloatPointInfluenceSource(1f, VectorXY.One, 20f)
        };

        var field = new InfluenceField<FloatPointInfluenceSource, float>(
            new SourceCountSampler(),
            sources,
            new FixedCuller(new List<FloatPointInfluenceSource> { sources[0] }));

        float value = field.Sample(new VectorXY(3f, 4f));

        Assert.That(value, Is.EqualTo(1f));
    }

    [Test]
    public void Constructor_WhenInfluenceSourcesContainNull_Throws()
    {
        var sources = new FloatPointInfluenceSource[] { null! };

        var exception = Assert.Throws<ArgumentException>(() =>
            new InfluenceField<FloatPointInfluenceSource, float>(
                new SourceCountSampler(),
                sources));

        Assert.That(exception!.ParamName, Is.EqualTo("influenceSources"));
    }

    [Test]
    public void Sample_WhenCullerReturnsNull_ThrowsInvalidOperationException()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, VectorXY.Zero, 10f)
        };

        var field = new InfluenceField<FloatPointInfluenceSource, float>(
            new SourceCountSampler(),
            sources,
            new FixedCuller(null));

        var exception = Assert.Throws<InvalidOperationException>(() => field.Sample(VectorXY.Zero));

        Assert.That(exception!.Message, Does.Contain("returned null"));
    }

    [Test]
    public void Sample_WhenCullerReturnsEmptyList_ThrowsInvalidOperationException()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, VectorXY.Zero, 10f)
        };

        var field = new InfluenceField<FloatPointInfluenceSource, float>(
            new SourceCountSampler(),
            sources,
            new FixedCuller(new List<FloatPointInfluenceSource>()));

        var exception = Assert.Throws<InvalidOperationException>(() => field.Sample(VectorXY.Zero));

        Assert.That(exception!.Message, Does.Contain("empty source list"));
    }

    [Test]
    public void Constructor_WhenHalfPlaneCullerSourcePointsAreEmpty_Throws()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new HalfPlaneCuller<FloatPointInfluenceSource>(Array.Empty<FloatPointInfluenceSource>()));

        Assert.That(exception!.ParamName, Is.EqualTo("sourcePoints"));
    }

    private sealed class SourceCountSampler : IInfluenceSampler<FloatPointInfluenceSource, float>
    {
        public float Sample(IReadOnlyList<FloatPointInfluenceSource> influenceSources, VectorXY point)
        {
            return influenceSources.Count;
        }
    }

    private sealed class FixedCuller : IInfluenceSourceCuller<FloatPointInfluenceSource>
    {
        private readonly List<FloatPointInfluenceSource>? _sources;

        public FixedCuller(List<FloatPointInfluenceSource>? sources)
        {
            _sources = sources;
        }

        public List<FloatPointInfluenceSource> Cull(VectorXY point)
        {
            return _sources!;
        }
    }
}
