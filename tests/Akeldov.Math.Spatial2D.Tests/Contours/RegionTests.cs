using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Contours;

public class RegionTests
{
    [Test]
    public void Constructor_WhenContoursIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Region(null!));
    }

    [Test]
    public void Constructor_WhenContoursIsEmpty_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Region(Array.Empty<IContour>()));
    }

    [Test]
    public void Constructor_WhenContoursContainsNull_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Region(new IContour[] { null! }));
    }

    [Test]
    public void Constructor_WhenFillRuleIsUnsupported_Throws()
    {
        var contour = CreateSquareContour(0f, 0f, 1f, 1f);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Region(new IContour[] { contour }, (FillRule)42));

        Assert.That(exception!.ParamName, Is.EqualTo("fillRule"));
    }

    [Test]
    public void Constructor_WhenContoursIntersect_Throws()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Region(new IContour[]
            {
                CreateSquareContour(0f, 0f, 4f, 4f),
                CreateSquareContour(2f, -1f, 5f, 2f)
            }));

        Assert.That(exception!.ParamName, Is.EqualTo("contours"));
    }

    [Test]
    public void Constructor_WhenContoursTouch_Throws()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Region(new IContour[]
            {
                CreateSquareContour(0f, 0f, 4f, 4f),
                CreateSquareContour(4f, 1f, 5f, 3f)
            }));

        Assert.That(exception!.ParamName, Is.EqualTo("contours"));
    }

    [Test]
    public void Contours_WhenAccessed_ReturnsReadOnlyView()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 1f, 1f)
        });

        Assert.That(region.Contours, Is.Not.InstanceOf<IContour[]>());
        Assert.Throws<NotSupportedException>(() =>
            ((IList<IContour>)region.Contours)[0] = CreateSquareContour(1f, 1f, 2f, 2f));
    }

    [Test]
    public void Contains_WhenPointIsInsideOuterContourAndOutsideHole_ReturnsTrue()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(0.5f, 0.5f)), Is.True);
    }

    [Test]
    public void Contains_WhenPointIsInsideHole_ReturnsFalse()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(2f, 2f)), Is.False);
    }

    [Test]
    public void Contains_WhenPointIsOnHoleBoundary_ReturnsTrue()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(1f, 2f)), Is.True);
    }

    [Test]
    public void Contains_WhenRegionIsSquareWithSquareHole_ClassifiesPoints()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(-0.5f, 2f)), Is.False);
        Assert.That(region.Contains(new VectorXY(0.5f, 0.5f)), Is.True);
        Assert.That(region.Contains(new VectorXY(2f, 2f)), Is.False);
        Assert.That(region.Contains(new VectorXY(0f, 2f)), Is.True);
        Assert.That(region.Contains(new VectorXY(1f, 2f)), Is.True);
    }

    [Test]
    public void Contains_WhenContoursAreNested_AlternatesInsideAndOutside()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 8f, 8f),
            CreateSquareContour(1f, 1f, 7f, 7f),
            CreateSquareContour(2f, 2f, 6f, 6f),
            CreateSquareContour(3f, 3f, 5f, 5f)
        });

        Assert.That(region.Contains(new VectorXY(0.5f, 0.5f)), Is.True);
        Assert.That(region.Contains(new VectorXY(1.5f, 1.5f)), Is.False);
        Assert.That(region.Contains(new VectorXY(2.5f, 2.5f)), Is.True);
        Assert.That(region.Contains(new VectorXY(4f, 4f)), Is.False);
        Assert.That(region.Contains(new VectorXY(8.5f, 8.5f)), Is.False);
    }

    [Test]
    public void Contains_WhenPointCoordinateIsInvalid_Throws()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 1f, 1f)
        });

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            region.Contains(new VectorXY(float.NaN, 0f)));

        Assert.That(exception!.ParamName, Is.EqualTo("point"));
    }

    private static Contour CreateSquareContour(float left, float bottom, float right, float top)
    {
        return new Contour(new IBoundedParameterizedCurve[]
        {
            new Segment(new VectorXY(left, bottom), new VectorXY(right, bottom)),
            new Segment(new VectorXY(right, bottom), new VectorXY(right, top)),
            new Segment(new VectorXY(right, top), new VectorXY(left, top)),
            new Segment(new VectorXY(left, top), new VectorXY(left, bottom))
        });
    }
}
