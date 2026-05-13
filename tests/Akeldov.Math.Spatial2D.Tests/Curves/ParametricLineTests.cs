using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class ParametricLineTests
{
    [Test]
    public void Constructor_WhenDirectionIsZero_Throws()
    {
        Assert.Throws<ArgumentException>(() => new ParametricLine(VectorXY.Zero, VectorXY.Zero));
    }

    [TestCase(float.NaN, 0f, "origin")]
    [TestCase(0f, float.NaN, "origin")]
    [TestCase(float.PositiveInfinity, 0f, "origin")]
    [TestCase(0f, float.NegativeInfinity, "origin")]
    [TestCase(float.NaN, 0f, "direction")]
    [TestCase(0f, float.NaN, "direction")]
    [TestCase(float.PositiveInfinity, 0f, "direction")]
    [TestCase(0f, float.NegativeInfinity, "direction")]
    public void Constructor_WhenOriginOrDirectionCoordinateIsInvalid_Throws(float x, float y, string paramName)
    {
        VectorXY origin = paramName == "origin" ? new VectorXY(x, y) : VectorXY.Zero;
        VectorXY direction = paramName == "direction" ? new VectorXY(x, y) : new VectorXY(1f, 0f);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ParametricLine(origin, direction));

        Assert.That(exception!.ParamName, Is.EqualTo(paramName));
    }

    [Test]
    public void Constructor_WhenReferencePointCoordinateIsInvalid_Throws()
    {
        var line = default(Line);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ParametricLine(line, new VectorXY(float.NaN, 0f)));

        Assert.That(exception!.ParamName, Is.EqualTo("referencePoint"));
    }

    [Test]
    public void Constructor_WhenDirectionIsNotParallelToLine_Throws()
    {
        var line = new Line(new VectorXY(0f, 0f), new VectorXY(2f, 0f));

        Assert.Throws<ArgumentException>(() => new ParametricLine(line, VectorXY.Zero, new VectorXY(0f, 1f)));
    }

    [Test]
    public void Constructor_WithOriginAndDirection_UsesOriginAndNormalizedDirection()
    {
        var line = new ParametricLine(new VectorXY(2f, 3f), new VectorXY(2f, 0f));

        AssertVector(line.Origin, 2f, 3f);
        AssertVector(line.Direction, 1f, 0f);
        AssertVector(line.Line.ClosestPointToOrigin, 0f, 3f);
    }

    [Test]
    public void DefaultParametricLine_RepresentsHorizontalXAxis()
    {
        var line = default(ParametricLine);
        var sameLine = new ParametricLine(default(Line));

        Assert.That(line, Is.EqualTo(sameLine));
        Assert.That(line.Line, Is.EqualTo(default(Line)));
        AssertVector(line.Origin, 0f, 0f);
        AssertVector(line.Direction, 1f, 0f);
        AssertVector(line.Normal, 0f, 1f);

        var projection = line.ProjectWithParameter(new VectorXY(3f, 4f));
        AssertVector(projection.ProjectedPoint, 3f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(3f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(4f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsGlobalZero_UsesClosestPointToGlobalOrigin()
    {
        var line = new ParametricLine(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.GlobalZero);

        AssertVector(line.Origin, 0f, 3f);
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsPointA_UsesPointA()
    {
        var line = new ParametricLine(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.PointA);

        AssertVector(line.Origin, 2f, 3f);
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsPointB_UsesPointB()
    {
        var line = new ParametricLine(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.PointB);

        AssertVector(line.Origin, 4f, 3f);
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsMidpoint_UsesMidpoint()
    {
        var line = new ParametricLine(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.Midpoint);

        AssertVector(line.Origin, 3f, 3f);
    }

    [Test]
    public void ProjectWithParameter_WhenReferencePointIsProvided_MeasuresCurveCoordinateFromItsProjection()
    {
        var line = new ParametricLine(new VectorXY(2f, 0f), new VectorXY(4f, 0f), new VectorXY(2f, 5f));

        var projection = line.ProjectWithParameter(VectorXY.Zero);

        AssertVector(line.Origin, 2f, 0f);
        AssertVector(projection.ProjectedPoint, 0f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(-2f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void ProjectWithParameter_WhenDirectionIsReversed_MeasuresCurveCoordinateInReversedDirection()
    {
        var geometricLine = new Line(new VectorXY(0f, 0f), new VectorXY(4f, 0f));
        var line = new ParametricLine(geometricLine, VectorXY.Zero, new VectorXY(-1f, 0f));

        var projection = line.ProjectWithParameter(new VectorXY(2f, 1f));

        AssertVector(line.Direction, -1f, 0f);
        AssertVector(projection.ProjectedPoint, 2f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(-2f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(1f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Equals_WhenOriginDiffers_ReturnsFalse()
    {
        var geometricLine = new Line(new VectorXY(0f, 0f), new VectorXY(4f, 0f));
        var first = new ParametricLine(geometricLine, VectorXY.Zero);
        var second = new ParametricLine(geometricLine, new VectorXY(2f, 0f));

        Assert.That(first.Equals(second), Is.False);
        Assert.That(first.HasSameGeometry(second), Is.True);
    }

    [Test]
    public void Equals_WhenDirectionDiffers_ReturnsFalse()
    {
        var geometricLine = new Line(new VectorXY(0f, 0f), new VectorXY(4f, 0f));
        var first = new ParametricLine(geometricLine, VectorXY.Zero, new VectorXY(1f, 0f));
        var second = new ParametricLine(geometricLine, VectorXY.Zero, new VectorXY(-1f, 0f));

        Assert.That(first.Equals(second), Is.False);
        Assert.That(first.HasSameGeometry(second), Is.True);
    }

    [Test]
    public void ExplicitConversionToLine_ReturnsGeometricLine()
    {
        var geometricLine = new Line(new VectorXY(0f, 0f), new VectorXY(4f, 0f));
        var line = new ParametricLine(geometricLine, new VectorXY(2f, 0f));

        Line converted = (Line)line;

        Assert.That(converted, Is.EqualTo(geometricLine));
        Assert.That(line.HasSameGeometry(converted), Is.True);
    }

    private static void AssertVector(VectorXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
