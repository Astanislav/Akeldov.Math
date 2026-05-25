namespace Akeldov.Math.Spatial2D.Tests.Centroid;

public class PositionedCollectionExtensionsTests
{
    [Test]
    public void GetCentroid_WithReadOnlyList_WhenItemsEmpty_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetCentroid());
    }

    [Test]
    public void GetCentroid_WithArray_WhenItemsEmpty_ThrowsArgumentException()
    {
        var items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetCentroid());
    }

    [Test]
    public void GetCentroid_WithReadOnlyList_WhenItemsContainNull_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = new PositionedItem[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            null!
        };

        AssertItemsArgumentException(() => items.GetCentroid());
    }

    [Test]
    public void GetCentroid_WithArray_WhenItemsContainNull_ThrowsArgumentException()
    {
        var items = new PositionedItem[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            null!
        };

        AssertItemsArgumentException(() => items.GetCentroid());
    }

    [Test]
    public void GetClosestToCentroid_WithReadOnlyList_WhenItemsEmpty_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetClosestToCentroid());
    }

    [Test]
    public void GetClosestToCentroid_WithArray_WhenItemsEmpty_ThrowsArgumentException()
    {
        var items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetClosestToCentroid());
    }

    [Test]
    public void GetClosestToCentroid_WithReadOnlyList_WhenItemsContainNull_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = new PositionedItem[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            null!
        };

        AssertItemsArgumentException(() => items.GetClosestToCentroid());
    }

    [Test]
    public void GetClosestToCentroid_WithArray_WhenItemsContainNull_ThrowsArgumentException()
    {
        var items = new PositionedItem[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            null!
        };

        AssertItemsArgumentException(() => items.GetClosestToCentroid());
    }

    [Test]
    public void GetClosestTo_WithReadOnlyList_WhenItemsEmpty_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetClosestTo(new PointXY(0f, 0f)));
    }

    [Test]
    public void GetClosestTo_WithArray_WhenItemsEmpty_ThrowsArgumentException()
    {
        var items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetClosestTo(new PointXY(0f, 0f)));
    }

    [Test]
    public void GetClosestTo_WithReadOnlyList_WhenItemsContainNull_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = new PositionedItem[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            null!
        };

        AssertItemsArgumentException(() => items.GetClosestTo(new PointXY(0f, 0f)));
    }

    [Test]
    public void GetClosestTo_WithArray_WhenItemsContainNull_ThrowsArgumentException()
    {
        var items = new PositionedItem[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            null!
        };

        AssertItemsArgumentException(() => items.GetClosestTo(new PointXY(0f, 0f)));
    }

    [Test]
    public void GetClosestToCentroid_WithReadOnlyList_WhenFirstItemIsClosestToCentroid_ReturnsFirstItem()
    {
        IReadOnlyList<PositionedItem> items = new[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            new PositionedItem("right", new PointXY(10f, 0f)),
            new PositionedItem("left", new PointXY(-10f, 0f))
        };

        var closest = items.GetClosestToCentroid();

        Assert.That(closest.Name, Is.EqualTo("center"));
    }

    [Test]
    public void GetClosestToCentroid_WithArray_WhenFirstItemIsClosestToCentroid_ReturnsFirstItem()
    {
        var items = new[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            new PositionedItem("right", new PointXY(10f, 0f)),
            new PositionedItem("left", new PointXY(-10f, 0f))
        };

        var closest = items.GetClosestToCentroid();

        Assert.That(closest.Name, Is.EqualTo("center"));
    }

    [Test]
    public void GetClosestTo_WithReadOnlyList_WhenFirstItemIsClosestToPoint_ReturnsFirstItem()
    {
        IReadOnlyList<PositionedItem> items = new[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            new PositionedItem("right", new PointXY(10f, 0f)),
            new PositionedItem("left", new PointXY(-10f, 0f))
        };

        var closest = items.GetClosestTo(new PointXY(0f, 0f));

        Assert.That(closest.Name, Is.EqualTo("center"));
    }

    [Test]
    public void GetClosestTo_WithArray_WhenFirstItemIsClosestToPoint_ReturnsFirstItem()
    {
        var items = new[]
        {
            new PositionedItem("center", new PointXY(0f, 0f)),
            new PositionedItem("right", new PointXY(10f, 0f)),
            new PositionedItem("left", new PointXY(-10f, 0f))
        };

        var closest = items.GetClosestTo(new PointXY(0f, 0f));

        Assert.That(closest.Name, Is.EqualTo("center"));
    }

    [Test]
    public void GetCentroid_WithPointArray_ReturnsCentroid()
    {
        var points = new[]
        {
            new PointXY(0f, 0f),
            new PointXY(10f, 0f),
            new PointXY(-4f, 6f)
        };

        PointXY centroid = points.GetCentroid();

        Assert.That(centroid, Is.EqualTo(new PointXY(2f, 2f)));
    }

    [Test]
    public void GetClosestTo_WithPointArray_ReturnsClosestPoint()
    {
        var points = new[]
        {
            new PointXY(0f, 0f),
            new PointXY(10f, 0f),
            new PointXY(-4f, 6f)
        };

        PointXY closest = points.GetClosestTo(new PointXY(9f, 1f));

        Assert.That(closest, Is.EqualTo(new PointXY(10f, 0f)));
    }

    private static void AssertItemsArgumentException(TestDelegate action)
    {
        var exception = Assert.Throws<ArgumentException>(action);

        Assert.That(exception!.ParamName, Is.EqualTo("items"));
    }

    private sealed class PositionedItem : IHasPosition2D
    {
        public PositionedItem(string name, PointXY position)
        {
            Name = name;
            Position = position;
        }

        public string Name { get; }

        public PointXY Position { get; }
    }
}
