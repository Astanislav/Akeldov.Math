using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class RayTests
{
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Constructor_WhenAngleIsInvalid_Throws(float angle)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Ray(VectorXY.Zero, angle));

        Assert.That(exception!.ParamName, Is.EqualTo("angleRad"));
    }

    [Test]
    public void RayIntersections_WhenThisRayOriginBelongsToOtherCollinearRay_ReturnsThisOrigin()
    {
        var ray = new Ray(new VectorXY(2f, 0f));
        var other = new Ray(VectorXY.Zero);

        var intersections = ray.GetRayIntersections(other);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 2f, 0f);
    }

    [Test]
    public void RayIntersections_WhenOtherCollinearRayStartsAhead_ReturnsOtherOrigin()
    {
        var ray = new Ray(VectorXY.Zero);
        var other = new Ray(new VectorXY(2f, 0f));

        var intersections = ray.GetRayIntersections(other);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 2f, 0f);
    }

    [Test]
    public void RayIntersections_WhenCollinearRaysFaceEachOther_ReturnsThisOrigin()
    {
        var ray = new Ray(VectorXY.Zero);
        var other = new Ray(new VectorXY(2f, 0f), MathF.PI);

        var intersections = ray.GetRayIntersections(other);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 0f, 0f);
    }

    [Test]
    public void RayIntersections_WhenRaysAreParallelButNotCollinear_ReturnsEmpty()
    {
        var ray = new Ray(VectorXY.Zero);
        var other = new Ray(new VectorXY(0f, 1f));

        var intersections = ray.GetRayIntersections(other);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void RayIntersections_WhenNonParallelRaysCrossAhead_ReturnsIntersection()
    {
        var ray = new Ray(VectorXY.Zero);
        var other = new Ray(new VectorXY(2f, -1f), MathF.PI / 2f);

        var intersections = ray.GetRayIntersections(other);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 2f, 0f);
    }

    [Test]
    public void RayIntersections_WhenIntersectionIsBehindOneRay_ReturnsEmpty()
    {
        var ray = new Ray(VectorXY.Zero);
        var other = new Ray(new VectorXY(-2f, -1f), MathF.PI / 2f);

        var intersections = ray.GetRayIntersections(other);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void Project_WhenPointIsBehindRay_ClampsToOrigin()
    {
        var ray = new Ray(new VectorXY(1f, 0f));

        var projection = ray.Project(VectorXY.Zero);

        AssertVector(projection.Point, 1f, 0f);
        Assert.That(projection.Parameter, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(1f).Within(GeometryConstants.GeometryEpsilon));
    }

    private static void AssertVector(VectorXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
