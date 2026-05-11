using Akeldov.Math.Spatial2D.Partitioning.Voronoi;

namespace Akeldov.Math.Spatial2D.Tests.Partitioning.Voronoi;

public class VoronoiPartitionerTests
{
    [Test]
    public void Constructor_WhenSitesIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new VoronoiPartitioner<TestItem>(null!));
    }

    [Test]
    public void Constructor_WhenSitesIsEmpty_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new VoronoiPartitioner<TestItem>(Array.Empty<Site>()));
    }

    [Test]
    public void Constructor_WhenRelaxationIterationsIsNegative_Throws()
    {
        var sites = new[] { new Site(VectorXY.Zero, 1f) };

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new VoronoiPartitioner<TestItem>(sites, relaxationIterations: -1, EmptyCellPolicy.ThrowException));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void SiteConstructor_WhenPowerIsNegativeOrNaN_Throws(float power)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Site(VectorXY.Zero, power));
    }

    [Test]
    public void SiteConstructor_WhenPowerIsZero_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new Site(VectorXY.Zero, 0f));
    }

    [Test]
    public void SiteConstructor_WhenPowerIsPositiveInfinity_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new Site(VectorXY.Zero, float.PositiveInfinity));
    }

    [Test]
    public void Partition_AssignsEachItemToClosestSite()
    {
        var sites = new[]
        {
            new Site(new VectorXY(0f, 0f), 1f),
            new Site(new VectorXY(10f, 0f), 1f)
        };
        var texels = new[]
        {
            new TestItem("left", new VectorXY(1f, 0f)),
            new TestItem("right", new VectorXY(9f, 0f))
        };
        var partitioner = new VoronoiPartitioner<TestItem>(sites);

        var cells = partitioner.Partition(texels);

        Assert.That(cells, Has.Count.EqualTo(2));
        Assert.That(cells[0].Items.Select(item => item.Id), Is.EqualTo(new[] { "left" }));
        Assert.That(cells[1].Items.Select(item => item.Id), Is.EqualTo(new[] { "right" }));
    }

    [Test]
    public void Partition_WhenItemsContainNull_Throws()
    {
        var sites = new[] { new Site(VectorXY.Zero, 1f) };
        var texels = new TestItem[] { null! };
        var partitioner = new VoronoiPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var exception = Assert.Throws<ArgumentException>(() => partitioner.Partition(texels));

        Assert.That(exception!.ParamName, Is.EqualTo("items"));
    }

    [Test]
    public void Partition_WhenCellItemsAccessed_ReturnsReadOnlyView()
    {
        var sites = new[] { new Site(VectorXY.Zero, 1f) };
        var texels = new[] { new TestItem("item", VectorXY.Zero) };
        var partitioner = new VoronoiPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var cells = partitioner.Partition(texels);

        Assert.That(cells[0].Items, Is.Not.InstanceOf<List<TestItem>>());
        Assert.Throws<NotSupportedException>(() =>
            ((IList<TestItem>)cells[0].Items)[0] = new TestItem("replacement", VectorXY.One));
    }

    [Test]
    public void VoronoiCell_WhenItemListChangesAfterConstruction_UsesOriginalItems()
    {
        var items = new List<TestItem> { new TestItem("original", VectorXY.Zero) };
        var cell = new VoronoiCell<TestItem>(new Site(VectorXY.Zero, 1f), items);

        items.Clear();

        Assert.That(cell.Items, Has.Count.EqualTo(1));
        Assert.That(cell.Items[0].Id, Is.EqualTo("original"));
    }

    [Test]
    public void VoronoiCell_WhenItemsContainNull_Throws()
    {
        var items = new TestItem[] { null! };

        var exception = Assert.Throws<ArgumentException>(() =>
            new VoronoiCell<TestItem>(new Site(VectorXY.Zero, 1f), items));

        Assert.That(exception!.ParamName, Is.EqualTo("items"));
    }

    [Test]
    public void Partition_WhenSiteHasLargerPower_AssignsFartherItemToIt()
    {
        var sites = new[]
        {
            new Site(new VectorXY(0f, 0f), 1f),
            new Site(new VectorXY(10f, 0f), 3f)
        };
        var texels = new[]
        {
            new TestItem("weighted-right", new VectorXY(3f, 0f))
        };
        var partitioner = new VoronoiPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var cells = partitioner.Partition(texels);

        Assert.That(cells[0].Items, Is.Empty);
        Assert.That(cells[1].Items.Select(item => item.Id), Is.EqualTo(new[] { "weighted-right" }));
    }

    [Test]
    public void Partition_WhenSitePowerIsZero_AssignsOnlyExactSitePointToIt()
    {
        var sites = new[]
        {
            new Site(new VectorXY(0f, 0f), 0f),
            new Site(new VectorXY(10f, 0f), 1f)
        };
        var texels = new[]
        {
            new TestItem("exact-zero", VectorXY.Zero),
            new TestItem("near-zero", new VectorXY(1f, 0f))
        };
        var partitioner = new VoronoiPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var cells = partitioner.Partition(texels);

        Assert.That(cells[0].Items.Select(item => item.Id), Is.EqualTo(new[] { "exact-zero" }));
        Assert.That(cells[1].Items.Select(item => item.Id), Is.EqualTo(new[] { "near-zero" }));
    }

    [Test]
    public void Partition_WhenSitePowerIsPositiveInfinity_AssignsFinitePointsToIt()
    {
        var sites = new[]
        {
            new Site(new VectorXY(0f, 0f), 1f),
            new Site(new VectorXY(10f, 0f), float.PositiveInfinity)
        };
        var texels = new[]
        {
            new TestItem("far-from-infinite", new VectorXY(-100f, 0f)),
            new TestItem("near-infinite", new VectorXY(9f, 0f))
        };
        var partitioner = new VoronoiPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var cells = partitioner.Partition(texels);

        Assert.That(cells[0].Items, Is.Empty);
        Assert.That(cells[1].Items.Select(item => item.Id), Is.EqualTo(new[] { "far-from-infinite", "near-infinite" }));
    }

    [Test]
    public void Partition_WhenTexelsIsNull_Throws()
    {
        var sites = new[] { new Site(VectorXY.Zero, 1f) };
        var partitioner = new VoronoiPartitioner<TestItem>(sites);

        Assert.Throws<ArgumentNullException>(() => partitioner.Partition(null!));
    }

    [Test]
    public void Partition_WhenTexelsIsEmpty_Throws()
    {
        var sites = new[] { new Site(VectorXY.Zero, 1f) };
        var partitioner = new VoronoiPartitioner<TestItem>(sites);

        Assert.Throws<ArgumentOutOfRangeException>(() => partitioner.Partition(Array.Empty<TestItem>()));
    }

    [Test]
    public void Partition_WhenPolicyIsThrowExceptionAndCellIsEmpty_Throws()
    {
        var sites = new[]
        {
            new Site(new VectorXY(0f, 0f), 1f),
            new Site(new VectorXY(100f, 0f), 1f)
        };
        var texels = new[] { new TestItem("left", new VectorXY(1f, 0f)) };
        var partitioner = new VoronoiPartitioner<TestItem>(sites);

        Assert.Throws<InvalidOperationException>(() => partitioner.Partition(texels));
    }

    [Test]
    public void Partition_WhenPolicyIsExclude_RemovesEmptyCells()
    {
        var sites = new[]
        {
            new Site(new VectorXY(0f, 0f), 1f),
            new Site(new VectorXY(100f, 0f), 1f)
        };
        var texels = new[] { new TestItem("left", new VectorXY(1f, 0f)) };
        var partitioner = new VoronoiPartitioner<TestItem>(sites, EmptyCellPolicy.Exclude);

        var cells = partitioner.Partition(texels);

        Assert.That(cells, Has.Count.EqualTo(1));
        Assert.That(cells[0].Items.Single().Id, Is.EqualTo("left"));
    }

    [Test]
    public void Partition_WhenPolicyIsLeaveAsIs_ReturnsEmptyCells()
    {
        var sites = new[]
        {
            new Site(new VectorXY(0f, 0f), 1f),
            new Site(new VectorXY(100f, 0f), 1f)
        };
        var texels = new[] { new TestItem("left", new VectorXY(1f, 0f)) };
        var partitioner = new VoronoiPartitioner<TestItem>(sites, EmptyCellPolicy.LeaveAsIs);

        var cells = partitioner.Partition(texels);

        Assert.That(cells, Has.Count.EqualTo(2));
        Assert.That(cells[0].Items, Has.Count.EqualTo(1));
        Assert.That(cells[1].Items, Is.Empty);
    }

    private sealed class TestItem : IHasPosition2D
    {
        public TestItem(string id, VectorXY center)
        {
            Id = id;
            Center = center;
        }

        public string Id { get; }

        public VectorXY Center { get; }
    }
}
