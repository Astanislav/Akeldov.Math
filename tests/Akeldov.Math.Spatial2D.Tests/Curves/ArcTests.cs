using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class ArcTests
{
    [Test]
    public void Project_WhenPointAngleIsWithinWrappedArc_ProjectsToCircle()
    {
        var arc = new Arc(VectorXY.Zero, 2f, 3f * MathF.PI / 2f, MathF.PI / 2f);
        var point = new VectorXY(3f, 0f);

        var projection = arc.Project(point);

        AssertVector(projection.Point, 2f, 0f);
        Assert.That(projection.Distance, Is.EqualTo(1f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Constructor_WhenRadiusIsNegative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Arc(VectorXY.Zero, -1f, 0f, MathF.PI));
    }

    [Test]
    public void RayIntersections_WhenRayHitsArc_ReturnsIntersectionOnArc()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, MathF.PI / 2f);
        var ray = new Ray(new VectorXY(-1f, 0f));

        var intersections = arc.RayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 1f, 0f);
    }

    [Test]
    public void RayIntersections_WhenRayHitsCircleOutsideArc_ReturnsEmpty()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, MathF.PI / 2f);
        var ray = new Ray(new VectorXY(-2f, -1f));

        var intersections = arc.RayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void Distance_WhenPointIsNearArcEndpoint_UsesNearestEndpoint()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, MathF.PI / 2f);
        var point = new VectorXY(0f, 2f);

        var distance = arc.Distance(point);

        Assert.That(distance, Is.EqualTo(1f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenStartAndEndAnglesAreEqual_TreatsArcAsZeroLength()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, 0f);

        Assert.That(arc.IsFullCircle, Is.False);

        var projection = arc.Project(new VectorXY(0f, 1f));

        AssertVector(projection.Point, 1f, 0f);
        Assert.That(projection.Parameter, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(MathF.Sqrt(2f)).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void RayIntersections_WhenStartAndEndAnglesAreEqual_ReturnsOnlyZeroArcPoint()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, 0f);
        var ray = new Ray(new VectorXY(-2f, 0f));

        var intersections = arc.RayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 1f, 0f);
    }

    [Test]
    public void RayIntersections_WhenStartAndEndAnglesAreEqualAndRayMissesZeroArcPoint_ReturnsEmpty()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, 0f);
        var ray = new Ray(new VectorXY(0f, -2f), MathF.PI / 2f);

        var intersections = arc.RayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void Project_WhenStopAngleIsOneFullTurnAfterStart_TreatsArcAsFullCircle()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI);

        Assert.That(arc.IsFullCircle, Is.True);

        var projection = arc.Project(new VectorXY(0f, 2f));

        AssertVector(projection.Point, 0f, 1f);
        Assert.That(projection.Parameter, Is.EqualTo(0.25f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(1f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenPointIsAtArcCenter_ReturnsStartPoint()
    {
        var arc = new Arc(new VectorXY(1f, 1f), 2f, MathF.PI / 2f, MathF.PI);

        var projection = arc.Project(new VectorXY(1f, 1f));

        AssertVector(projection.Point, 1f, 3f);
        Assert.That(projection.Parameter, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(2f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void Project_WhenRadiusIsZero_ReturnsCenter()
    {
        var arc = new Arc(new VectorXY(1f, 1f), 0f, 0f, MathF.PI);

        var projection = arc.Project(new VectorXY(4f, 5f));

        AssertVector(projection.Point, 1f, 1f);
        Assert.That(projection.Distance, Is.EqualTo(5f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void RayIntersections_WhenStopAngleIsOneFullTurnAfterStart_ReturnsCircleIntersections()
    {
        var arc = new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI);
        var ray = new Ray(new VectorXY(-2f, 0f));

        var intersections = arc.RayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(2));
        AssertVector(intersections[0], -1f, 0f);
        AssertVector(intersections[1], 1f, 0f);
    }

    [Test]
    public void Equals_WhenOneArcIsZeroLengthAndOtherIsFullCircle_ReturnsFalse()
    {
        var zeroArc = new Arc(VectorXY.Zero, 1f, 0f, 0f);
        var fullCircle = new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI);

        Assert.That(zeroArc, Is.Not.EqualTo(fullCircle));
        Assert.That(zeroArc.GetHashCode(), Is.Not.EqualTo(fullCircle.GetHashCode()));
    }

    private static void AssertVector(VectorXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
