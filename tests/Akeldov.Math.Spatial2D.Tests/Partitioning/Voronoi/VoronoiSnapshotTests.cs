using Akeldov.Math.Spatial2D.Partitioning.Voronoi;

namespace Akeldov.Math.Spatial2D.Tests.Partitioning.Voronoi;

public class VoronoiSnapshotTests
{
    private static readonly VectorXY FieldSize = new VectorXY(120f, 60f);

    [Test]
    public void Partition_WithEqualWeightSites_MatchesApprovedImage()
    {
        var sites = new[]
        {
            new Site(new PointXY(25f, 30f), 1f),
            new Site(new PointXY(95f, 30f), 1f)
        };
        var texels = CreateGridTexels(FieldSize, 5f);
        var partitioner = new VoronoiItemPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var cells = partitioner.Partition(texels);

        Assert.That(cells[0].Items, Has.Count.EqualTo(144));
        Assert.That(cells[1].Items, Has.Count.EqualTo(144));
        AssertMatchesApprovedSvg("equal-site-weights.svg", cells, FieldSize);
    }

    [Test]
    public void Partition_WithWeightedSites_MatchesApprovedImage()
    {
        var sites = new[]
        {
            new Site(new PointXY(25f, 30f), 1f),
            new Site(new PointXY(95f, 30f), 2f)
        };
        var texels = CreateGridTexels(FieldSize, 5f);
        var partitioner = new VoronoiItemPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var cells = partitioner.Partition(texels);

        Assert.That(cells[0].Items, Has.Count.EqualTo(110));
        Assert.That(cells[1].Items, Has.Count.EqualTo(178));
        AssertMatchesApprovedSvg("weighted-sites.svg", cells, FieldSize);
    }

    private static TestItem[] CreateGridTexels(VectorXY fieldSize, float step)
    {
        var items = new List<TestItem>();

        for (float y = step * 0.5f; y < fieldSize.Y; y += step)
        {
            for (float x = step * 0.5f; x < fieldSize.X; x += step)
            {
                items.Add(new TestItem(new PointXY(x, y)));
            }
        }

        return items.ToArray();
    }

    private static void AssertMatchesApprovedSvg(
        string approvedFileName,
        IReadOnlyList<VoronoiItemPartition<TestItem>> result,
        VectorXY fieldSize)
    {
        string actual = VoronoiSvgRenderer.Render(result, fieldSize);
        string approvedPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Partitioning", "Voronoi", "Approved", approvedFileName);

        if (!File.Exists(approvedPath))
        {
            string actualPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, approvedFileName.Replace(".svg", ".actual.svg"));
            File.WriteAllText(actualPath, actual);
            TestContext.AddTestAttachment(actualPath, "Actual Voronoi image");
            Assert.Fail($"Voronoi approved image is missing. Actual image: {actualPath}");
        }

        string approved = File.ReadAllText(approvedPath);

        if (actual != approved)
        {
            string actualPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, approvedFileName.Replace(".svg", ".actual.svg"));
            File.WriteAllText(actualPath, actual);
            TestContext.AddTestAttachment(actualPath, "Actual Voronoi image");
            Assert.Fail($"Voronoi snapshot changed. Actual image: {actualPath}");
        }
    }

    public sealed class TestItem : IHasPosition2D
    {
        public TestItem(PointXY position)
        {
            Position = position;
        }

        public PointXY Position { get; }
    }
}
