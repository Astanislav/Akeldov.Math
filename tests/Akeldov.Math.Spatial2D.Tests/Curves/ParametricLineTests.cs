using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class ParametricLineTests
{
    [Test]
    public void Constructor_WhenDirectionIsZero_Throws()
    {
        Assert.Throws<ArgumentException>(() => new ParametricLine(VectorXY.Zero, VectorXY.Zero));
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
    public void Project_WhenReferencePointIsProvided_MeasuresCurveCoordinateFromItsProjection()
    {
        var line = new ParametricLine(new VectorXY(2f, 0f), new VectorXY(4f, 0f), new VectorXY(2f, 5f));

        var projection = line.Project(VectorXY.Zero);

        AssertVector(line.Origin, 2f, 0f);
        AssertVector(projection.ProjectedPoint, 0f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(-2f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenDirectionIsReversed_MeasuresCurveCoordinateInReversedDirection()
    {
        var geometricLine = new Line(new VectorXY(0f, 0f), new VectorXY(4f, 0f));
        var line = new ParametricLine(geometricLine, VectorXY.Zero, new VectorXY(-1f, 0f));

        var projection = line.Project(new VectorXY(2f, 1f));

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
