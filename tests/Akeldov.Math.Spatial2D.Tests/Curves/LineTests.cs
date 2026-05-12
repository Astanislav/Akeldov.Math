using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class LineTests
{
    [Test]
    public void Constructor_WhenPointsAreEqual_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Line(new VectorXY(2f, 3f), new VectorXY(2f, 3f)));
    }

    [Test]
    public void Constructor_WhenPointsAreAlmostEqual_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => new Line(VectorXY.Zero, new VectorXY(GeometryConstants.GeometryEpsilon * 0.5f, 0f)));
    }

    [Test]
    public void Constructor_WhenLinearEquationCoefficientsAreZero_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Line(0f, 0f, 1f));
    }

    [TestCase(float.NaN, 1f, 0f, "a")]
    [TestCase(float.PositiveInfinity, 1f, 0f, "a")]
    [TestCase(1f, float.NaN, 0f, "b")]
    [TestCase(1f, float.NegativeInfinity, 0f, "b")]
    [TestCase(1f, 0f, float.NaN, "c")]
    [TestCase(1f, 0f, float.PositiveInfinity, "c")]
    public void Constructor_WhenEquationCoefficientIsInvalid_Throws(
        float a,
        float b,
        float c,
        string paramName)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Line(a, b, c));

        Assert.That(exception!.ParamName, Is.EqualTo(paramName));
    }

    [Test]
    public void Constructor_FromEquation_NormalizesCoefficientsAndFixesSign()
    {
        var line = new Line(0f, -2f, 6f);

        Assert.That(line.EquationA, Is.EqualTo(0f));
        Assert.That(line.EquationB, Is.EqualTo(1f));
        Assert.That(line.EquationC, Is.EqualTo(-3f));
        AssertVector(line.Normal, 0f, 1f);
        AssertVector(line.Direction, 1f, 0f);
        AssertVector(line.ClosestPointToOrigin, 0f, 3f);
    }

    [Test]
    public void Equals_WhenSameLineIsBuiltFromDifferentPointPairs_ReturnsTrue()
    {
        var line = new Line(new VectorXY(0f, 3f), new VectorXY(2f, 3f));
        var sameLine = new Line(new VectorXY(-5f, 3f), new VectorXY(5f, 3f));

        Assert.That(line.Equals(sameLine), Is.True);
        Assert.That(line.GetHashCode(), Is.EqualTo(sameLine.GetHashCode()));
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsGlobalZero_UsesClosestPointToGlobalOrigin()
    {
        var line = new Line(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.GlobalZero);

        AssertVector(line.Origin, 0f, 3f);
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsPointA_UsesPointA()
    {
        var line = new Line(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.PointA);

        AssertVector(line.Origin, 2f, 3f);
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsPointB_UsesPointB()
    {
        var line = new Line(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.PointB);

        AssertVector(line.Origin, 4f, 3f);
    }

    [Test]
    public void Constructor_WhenReferencePointModeIsMiddle_UsesMidpoint()
    {
        var line = new Line(new VectorXY(2f, 3f), new VectorXY(4f, 3f), LineReferencePointMode.Middle);

        AssertVector(line.Origin, 3f, 3f);
    }

    [Test]
    public void RayIntersections_WhenRayLiesOnLine_ReturnsRayOrigin()
    {
        var line = new Line(new VectorXY(-5f, 0f), new VectorXY(5f, 0f));
        var ray = new Ray(new VectorXY(2f, 0f));

        var intersections = line.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 2f, 0f);
    }

    [Test]
    public void RayIntersections_WhenRayIsParallelButNotOnLine_ReturnsEmpty()
    {
        var line = new Line(new VectorXY(-5f, 1f), new VectorXY(5f, 1f));
        var ray = new Ray(VectorXY.Zero);

        var intersections = line.GetRayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void RayIntersections_WhenIntersectionIsBehindRay_ReturnsEmpty()
    {
        var line = new Line(new VectorXY(-1f, -1f), new VectorXY(-1f, 1f));
        var ray = new Ray(VectorXY.Zero);

        var intersections = line.GetRayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void Project_WhenDefaultReferencePointProjectsToGlobalOrigin_MeasuresCurveCoordinateFromGlobalOrigin()
    {
        var line = new Line(new VectorXY(2f, 0f), new VectorXY(4f, 0f));

        var projection = line.Project(VectorXY.Zero);

        AssertVector(projection.ProjectedPoint, 0f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenReferencePointIsProvided_MeasuresCurveCoordinateFromItsProjection()
    {
        var line = new Line(new VectorXY(2f, 0f), new VectorXY(4f, 0f), new VectorXY(2f, 5f));

        var projection = line.Project(VectorXY.Zero);

        AssertVector(line.Origin, 2f, 0f);
        AssertVector(projection.ProjectedPoint, 0f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(-2f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
    }

    private static void AssertVector(VectorXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
