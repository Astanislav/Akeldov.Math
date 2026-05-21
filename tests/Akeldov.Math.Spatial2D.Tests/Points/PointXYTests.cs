using System.Globalization;

namespace Akeldov.Math.Spatial2D.Tests.Points;

public class PointXYTests
{
    [Test]
    public void Subtract_WhenTwoPointsAreUsed_ReturnsVectorFromRightToLeft()
    {
        var from = new PointXY(1f, 2f);
        var to = new PointXY(4f, 6f);

        VectorXY vector = to - from;

        Assert.That(vector, Is.EqualTo(new VectorXY(3f, 4f)));
    }

    [Test]
    public void Add_WhenPointAndVectorAreUsed_ReturnsTranslatedPoint()
    {
        var point = new PointXY(1f, 2f);
        var vector = new VectorXY(3f, 4f);

        Assert.That(point + vector, Is.EqualTo(new PointXY(4f, 6f)));
        Assert.That(vector + point, Is.EqualTo(new PointXY(4f, 6f)));
    }

    [Test]
    public void Subtract_WhenPointAndVectorAreUsed_ReturnsTranslatedPoint()
    {
        var point = new PointXY(4f, 6f);
        var vector = new VectorXY(3f, 4f);

        Assert.That(point - vector, Is.EqualTo(new PointXY(1f, 2f)));
    }

    [Test]
    public void ExplicitConversionToVector_ReturnsCoordinateVector()
    {
        var point = new PointXY(1f, 2f);

        var vector = (VectorXY)point;

        Assert.That(vector, Is.EqualTo(new VectorXY(1f, 2f)));
    }

    [Test]
    public void ExplicitConversionFromVector_ReturnsPointWithVectorCoordinates()
    {
        var vector = new VectorXY(1f, 2f);

        var point = (PointXY)vector;

        Assert.That(point, Is.EqualTo(new PointXY(1f, 2f)));
    }

    [Test]
    public void Constructor_WhenCoordinatesAreFinite_StoresCoordinates()
    {
        var point = new PointXY(1f, -2f);

        Assert.That(point.X, Is.EqualTo(1f));
        Assert.That(point.Y, Is.EqualTo(-2f));
    }

    [Test]
    public void Constructor_WhenCoordinatesAreInfinite_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new PointXY(float.PositiveInfinity, 0f));
        Assert.DoesNotThrow(() => new PointXY(0f, float.NegativeInfinity));
    }

    [TestCase(float.NaN, 0f, "x")]
    [TestCase(0f, float.NaN, "y")]
    public void Constructor_WhenCoordinateIsNaN_Throws(float x, float y, string paramName)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new PointXY(x, y));

        Assert.That(exception!.ParamName, Is.EqualTo(paramName));
    }

    [Test]
    public void Deconstruct_ReturnsCoordinates()
    {
        var point = new PointXY(1f, 2f);

        var (x, y) = point;

        Assert.That(x, Is.EqualTo(1f));
        Assert.That(y, Is.EqualTo(2f));
    }

    [Test]
    public void ToString_UsesInvariantCulture()
    {
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        try
        {
            Assert.That(new PointXY(1.5f, 2.25f).ToString(), Is.EqualTo("(1.5, 2.25)"));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}
