using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class CircleTests
{
    [TestCase(float.PositiveInfinity, 0f)]
    [TestCase(0f, float.NegativeInfinity)]
    public void Constructor_WhenCenterCoordinateIsInvalid_Throws(float x, float y)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Circle(new PointXY(x, y), 1f));

        Assert.That(exception!.ParamName, Is.EqualTo("center"));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Constructor_WhenRadiusIsInvalid_Throws(float radius)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Circle(new PointXY(0f, 0f), radius));

        Assert.That(exception!.ParamName, Is.EqualTo("radius"));
    }

    [Test]
    public void Distance_WhenPointIsInsideCircle_ReturnsDistanceToCircumference()
    {
        var circle = new Circle(new PointXY(0f, 0f), 5f);

        var distance = circle.Distance(new PointXY(3f, 0f));

        Assert.That(distance, Is.EqualTo(2f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenPointIsOutsideCircle_ReturnsNearestPointOnCircumference()
    {
        var circle = new Circle(new PointXY(0f, 0f), 2f);

        var projection = circle.Project(new PointXY(3f, 0f));

        AssertVector(projection.ProjectedPoint, 2f, 0f);
        Assert.That(projection.Distance, Is.EqualTo(1f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenPointIsAtCenter_ReturnsPointOnPositiveXAxis()
    {
        var circle = new Circle(new PointXY(1f, 1f), 2f);

        var projection = circle.Project(new PointXY(1f, 1f));

        AssertVector(projection.ProjectedPoint, 3f, 1f);
        Assert.That(projection.Distance, Is.EqualTo(2f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenPointCoordinateIsInvalid_Throws()
    {
        var circle = new Circle(new PointXY(0f, 0f), 1f);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            circle.Project(new PointXY(float.PositiveInfinity, 0f)));

        Assert.That(exception!.ParamName, Is.EqualTo("point"));
    }

    [Test]
    public void RayIntersections_WhenRayStartsInsideCircle_ReturnsForwardExitPoint()
    {
        var circle = new Circle(new PointXY(0f, 0f), 2f);
        var ray = new Ray(new PointXY(0f, 0f));

        var intersections = circle.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 2f, 0f);
    }

    [Test]
    public void RayIntersections_WhenRayIsTangent_ReturnsSingleIntersection()
    {
        var circle = new Circle(new PointXY(0f, 0f), 1f);
        var ray = new Ray(new PointXY(-2f, 1f));

        var intersections = circle.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 0f, 1f);
    }

    [Test]
    public void RayIntersections_WithCustomGeometryEpsilon_WhenRayNearlyTouchesCircle_ReturnsSingleIntersection()
    {
        const float geometryEpsilon = 0.001f;
        var circle = new Circle(new PointXY(0f, 0f), 1f);
        var ray = new Ray(new PointXY(-2f, 1.00005f));

        var defaultIntersections = circle.GetRayIntersections(ray);
        var tolerantIntersections = circle.GetRayIntersections(ray, geometryEpsilon);

        Assert.That(defaultIntersections, Is.Empty);
        Assert.That(tolerantIntersections, Has.Count.EqualTo(1));
        AssertVector(tolerantIntersections[0], 0f, 1.00005f);
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void RayIntersections_WhenGeometryEpsilonIsInvalid_Throws(float geometryEpsilon)
    {
        var circle = new Circle(new PointXY(0f, 0f), 1f);
        var ray = new Ray(new PointXY(0f, 0f));

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            circle.GetRayIntersections(ray, geometryEpsilon));

        Assert.That(exception!.ParamName, Is.EqualTo("geometryEpsilon"));
    }

    [Test]
    public void RayIntersections_WhenCircleIsBehindRay_ReturnsEmpty()
    {
        var circle = new Circle(new PointXY(0f, 0f), 1f);
        var ray = new Ray(new PointXY(2f, 0f));

        var intersections = circle.GetRayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    private static void AssertVector(PointXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
