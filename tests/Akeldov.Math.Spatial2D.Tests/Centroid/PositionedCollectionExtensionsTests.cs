namespace Akeldov.Math.Spatial2D.Tests.Centroid;

public class PositionedCollectionExtensionsTests
{
    [Test]
    public void GetBarycenter_WithReadOnlyList_WhenItemsEmpty_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetBarycenter());
    }

    [Test]
    public void GetBarycenter_WithArray_WhenItemsEmpty_ThrowsArgumentException()
    {
        var items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetBarycenter());
    }

    [Test]
    public void GetBarycentric_WithReadOnlyList_WhenItemsEmpty_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetBarycentric());
    }

    [Test]
    public void GetBarycentric_WithArray_WhenItemsEmpty_ThrowsArgumentException()
    {
        var items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetBarycentric());
    }

    [Test]
    public void GetClosestTo_WithReadOnlyList_WhenItemsEmpty_ThrowsArgumentException()
    {
        IReadOnlyList<PositionedItem> items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetClosestTo(VectorXY.Zero));
    }

    [Test]
    public void GetClosestTo_WithArray_WhenItemsEmpty_ThrowsArgumentException()
    {
        var items = Array.Empty<PositionedItem>();

        AssertItemsArgumentException(() => items.GetClosestTo(VectorXY.Zero));
    }

    [Test]
    public void GetBarycentric_WithReadOnlyList_WhenFirstItemIsClosestToBarycenter_ReturnsFirstItem()
    {
        IReadOnlyList<PositionedItem> items = new[]
        {
            new PositionedItem("center", VectorXY.Zero),
            new PositionedItem("right", new VectorXY(10f, 0f)),
            new PositionedItem("left", new VectorXY(-10f, 0f))
        };

        var barycentric = items.GetBarycentric();

        Assert.That(barycentric.Name, Is.EqualTo("center"));
    }

    [Test]
    public void GetBarycentric_WithArray_WhenFirstItemIsClosestToBarycenter_ReturnsFirstItem()
    {
        var items = new[]
        {
            new PositionedItem("center", VectorXY.Zero),
            new PositionedItem("right", new VectorXY(10f, 0f)),
            new PositionedItem("left", new VectorXY(-10f, 0f))
        };

        var barycentric = items.GetBarycentric();

        Assert.That(barycentric.Name, Is.EqualTo("center"));
    }

    [Test]
    public void GetClosestTo_WithReadOnlyList_WhenFirstItemIsClosestToPoint_ReturnsFirstItem()
    {
        IReadOnlyList<PositionedItem> items = new[]
        {
            new PositionedItem("center", VectorXY.Zero),
            new PositionedItem("right", new VectorXY(10f, 0f)),
            new PositionedItem("left", new VectorXY(-10f, 0f))
        };

        var closest = items.GetClosestTo(VectorXY.Zero);

        Assert.That(closest.Name, Is.EqualTo("center"));
    }

    [Test]
    public void GetClosestTo_WithArray_WhenFirstItemIsClosestToPoint_ReturnsFirstItem()
    {
        var items = new[]
        {
            new PositionedItem("center", VectorXY.Zero),
            new PositionedItem("right", new VectorXY(10f, 0f)),
            new PositionedItem("left", new VectorXY(-10f, 0f))
        };

        var closest = items.GetClosestTo(VectorXY.Zero);

        Assert.That(closest.Name, Is.EqualTo("center"));
    }

    private static void AssertItemsArgumentException(TestDelegate action)
    {
        var exception = Assert.Throws<ArgumentException>(action);

        Assert.That(exception!.ParamName, Is.EqualTo("items"));
    }

    private sealed class PositionedItem : IHasPosition2D
    {
        public PositionedItem(string name, VectorXY center)
        {
            Name = name;
            Position = center;
        }

        public string Name { get; }

        public VectorXY Position { get; }
    }
}
