namespace Akeldov.Math.Spatial2D.Tests.Centroid;

public class PositionedCollectionExtensionsTests
{
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

    private sealed class PositionedItem : IHasPosition2D
    {
        public PositionedItem(string name, VectorXY center)
        {
            Name = name;
            Center = center;
        }

        public string Name { get; }

        public VectorXY Center { get; }
    }
}
