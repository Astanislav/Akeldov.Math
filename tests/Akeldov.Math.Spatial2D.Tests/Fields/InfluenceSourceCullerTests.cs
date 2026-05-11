using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class InfluenceSourceCullerTests
{
    [Test]
    public void HalfPlaneCuller_WithSingleSource_ReturnsThatSource()
    {
        var source = new FloatPointInfluenceSource(1f, VectorXY.Zero, 10f);
        var culler = new HalfPlaneCuller<FloatPointInfluenceSource>(new[] { source });

        List<FloatPointInfluenceSource> selectedSources = culler.Cull(new VectorXY(3f, 4f));

        Assert.That(selectedSources, Has.Count.EqualTo(1));
        Assert.That(selectedSources[0], Is.SameAs(source));
    }

    [Test]
    public void HalfPlaneCuller_WhenSourceListChangesAfterConstruction_UsesOriginalSources()
    {
        var source = new FloatPointInfluenceSource(1f, VectorXY.Zero, 10f);
        var sources = new List<FloatPointInfluenceSource> { source };
        var culler = new HalfPlaneCuller<FloatPointInfluenceSource>(sources);

        sources.Clear();

        List<FloatPointInfluenceSource> selectedSources = culler.Cull(new VectorXY(3f, 4f));

        Assert.That(selectedSources, Has.Count.EqualTo(1));
        Assert.That(selectedSources[0], Is.SameAs(source));
    }

    [Test]
    public void HalfPlaneCuller_WhenSourcePointsContainNull_Throws()
    {
        var sources = new FloatPointInfluenceSource[] { null! };

        var exception = Assert.Throws<ArgumentException>(() =>
            new HalfPlaneCuller<FloatPointInfluenceSource>(sources));

        Assert.That(exception!.ParamName, Is.EqualTo("sourcePoints"));
    }

    [Test]
    public void DelaunayCuller_WithTriangleContainingPoint_ReturnsTriangleSources()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 0f), 1f),
            new FloatPointInfluenceSource(1f, new VectorXY(10f, 0f), 2f),
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 10f), 3f)
        };

        var culler = new DelaunayCuller<FloatPointInfluenceSource>(sources);

        List<FloatPointInfluenceSource> selectedSources = culler.Cull(new VectorXY(2f, 3f));

        Assert.That(selectedSources, Has.Count.EqualTo(3));
        Assert.That(selectedSources, Does.Contain(sources[0]));
        Assert.That(selectedSources, Does.Contain(sources[1]));
        Assert.That(selectedSources, Does.Contain(sources[2]));
    }

    [Test]
    public void DelaunayCuller_WhenSourcePointsContainNull_Throws()
    {
        var sources = new FloatPointInfluenceSource[]
        {
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 0f), 1f),
            null!,
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 10f), 3f)
        };

        var exception = Assert.Throws<ArgumentException>(() =>
            new DelaunayCuller<FloatPointInfluenceSource>(sources));

        Assert.That(exception!.ParamName, Is.EqualTo("sourcePoints"));
    }

    [Test]
    public void DelaunayCuller_WhenSourcePointsAreCollinear_UsesHalfPlaneCuller()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 0f), 1f),
            new FloatPointInfluenceSource(1f, new VectorXY(10f, 0f), 2f),
            new FloatPointInfluenceSource(1f, new VectorXY(20f, 0f), 3f),
            new FloatPointInfluenceSource(1f, new VectorXY(100f, 0f), 4f)
        };

        var culler = new DelaunayCuller<FloatPointInfluenceSource>(sources);
        var halfPlaneCuller = new HalfPlaneCuller<FloatPointInfluenceSource>(sources);

        List<FloatPointInfluenceSource> selectedSources = culler.Cull(new VectorXY(12f, 0f));
        List<FloatPointInfluenceSource> expectedSources = halfPlaneCuller.Cull(new VectorXY(12f, 0f));

        AssertSameSources(selectedSources, expectedSources);
        Assert.That(selectedSources, Does.Not.Contain(sources[3]));
    }

    [Test]
    public void DelaunayCuller_WhenPointIsOutsideTriangulationNearHullVertex_ReturnsHullVertex()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 0f), 1f),
            new FloatPointInfluenceSource(1f, new VectorXY(10f, 0f), 2f),
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 10f), 3f),
            new FloatPointInfluenceSource(1f, new VectorXY(10f, 10f), 4f),
            new FloatPointInfluenceSource(1f, new VectorXY(5f, 5f), 5f)
        };

        var culler = new DelaunayCuller<FloatPointInfluenceSource>(sources);

        List<FloatPointInfluenceSource> selectedSources = culler.Cull(new VectorXY(-5f, -5f));

        Assert.That(selectedSources, Has.Count.EqualTo(1));
        Assert.That(selectedSources[0], Is.SameAs(sources[0]));
    }

    [Test]
    public void DelaunayCuller_WhenPointIsOutsideTriangulationNearHullEdge_ReturnsHullEdge()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, new VectorXY(12f, 12f), 1f),
            new FloatPointInfluenceSource(1f, new VectorXY(88f, 14f), 2f),
            new FloatPointInfluenceSource(1f, new VectorXY(18f, 58f), 3f),
            new FloatPointInfluenceSource(1f, new VectorXY(83f, 54f), 4f),
            new FloatPointInfluenceSource(1f, new VectorXY(50f, 34f), 5f)
        };

        var point = new VectorXY(50f, 0f);
        var culler = new DelaunayCuller<FloatPointInfluenceSource>(sources);
        var halfPlaneCuller = new HalfPlaneCuller<FloatPointInfluenceSource>(sources);

        List<FloatPointInfluenceSource> halfPlaneSources = halfPlaneCuller.Cull(point);
        List<FloatPointInfluenceSource> selectedSources = culler.Cull(point);

        Assert.That(halfPlaneSources, Has.Count.GreaterThan(2));
        Assert.That(halfPlaneSources, Does.Contain(sources[4]));
        Assert.That(selectedSources, Has.Count.EqualTo(2));
        Assert.That(selectedSources, Does.Contain(sources[0]));
        Assert.That(selectedSources, Does.Contain(sources[1]));
        Assert.That(selectedSources, Does.Not.Contain(sources[4]));
    }

    [Test]
    public void DelaunayCuller_WhenSamplingAboveTopHullEdge_ReturnsThreeExpectedRegions()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, new VectorXY(12f, 12f), 1f),
            new FloatPointInfluenceSource(1f, new VectorXY(88f, 14f), 2f),
            new FloatPointInfluenceSource(1f, new VectorXY(18f, 58f), 3f),
            new FloatPointInfluenceSource(1f, new VectorXY(83f, 54f), 4f),
            new FloatPointInfluenceSource(1f, new VectorXY(50f, 34f), 5f)
        };

        var culler = new DelaunayCuller<FloatPointInfluenceSource>(sources);

        List<FloatPointInfluenceSource> leftRegion = culler.Cull(new VectorXY(0f, 0f));
        List<FloatPointInfluenceSource> edgeRegion = culler.Cull(new VectorXY(50f, 0f));
        List<FloatPointInfluenceSource> rightRegion = culler.Cull(new VectorXY(100f, 0f));

        Assert.That(leftRegion, Has.Count.EqualTo(1));
        Assert.That(leftRegion[0], Is.SameAs(sources[0]));
        Assert.That(edgeRegion, Has.Count.EqualTo(2));
        Assert.That(edgeRegion, Does.Contain(sources[0]));
        Assert.That(edgeRegion, Does.Contain(sources[1]));
        Assert.That(rightRegion, Has.Count.EqualTo(1));
        Assert.That(rightRegion[0], Is.SameAs(sources[1]));
    }

    [Test]
    public void DelaunayCuller_WhenSourcePointCountIsLessThanThree_Throws()
    {
        var sources = new[]
        {
            new FloatPointInfluenceSource(1f, new VectorXY(0f, 0f), 1f),
            new FloatPointInfluenceSource(1f, new VectorXY(10f, 0f), 2f)
        };

        var exception = Assert.Throws<ArgumentException>(
            () => new DelaunayCuller<FloatPointInfluenceSource>(sources));

        Assert.That(exception!.ParamName, Is.EqualTo("sourcePoints"));
    }

    private static void AssertSameSources(
        IReadOnlyList<FloatPointInfluenceSource> actual,
        IReadOnlyList<FloatPointInfluenceSource> expected)
    {
        Assert.That(actual, Has.Count.EqualTo(expected.Count));

        for (int i = 0; i < expected.Count; i++)
            Assert.That(actual[i], Is.SameAs(expected[i]));
    }
}
