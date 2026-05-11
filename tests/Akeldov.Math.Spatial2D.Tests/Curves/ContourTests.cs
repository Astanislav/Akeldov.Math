using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class ContourTests
{
    [Test]
    public void Constructor_WhenCurvesIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Contour(null!));
    }

    [Test]
    public void Constructor_WhenCurvesIsEmpty_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Contour(Array.Empty<ICurve>()));
    }

    [Test]
    public void Constructor_WhenCurvesContainsNull_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Contour(new ICurve[] { null! }));
    }

    [Test]
    public void Contains_WhenPointIsInsideSegmentContour_ReturnsTrue()
    {
        var contour = new Contour(new ICurve[]
        {
            new Segment(new VectorXY(0f, 0f), new VectorXY(2f, 0f)),
            new Segment(new VectorXY(2f, 0f), new VectorXY(2f, 2f)),
            new Segment(new VectorXY(2f, 2f), new VectorXY(0f, 2f)),
            new Segment(new VectorXY(0f, 2f), new VectorXY(0f, 0f))
        });

        Assert.That(contour.Contains(new VectorXY(1f, 1f)), Is.True);
    }

    [Test]
    public void Contains_WhenPointIsOutsideContour_ReturnsFalse()
    {
        IContour contour = new Contour(new ICurve[] { new Circle(VectorXY.Zero, 1f) });

        bool isInside = contour.Contains(new VectorXY(2f, 0f));

        Assert.That(isInside, Is.False);
    }

    [Test]
    public void Contains_WhenPointIsOnContour_ReturnsTrue()
    {
        var contour = new Contour(new ICurve[] { new Circle(VectorXY.Zero, 1f) });

        Assert.That(contour.Contains(new VectorXY(1f, 0f)), Is.True);
    }
}
