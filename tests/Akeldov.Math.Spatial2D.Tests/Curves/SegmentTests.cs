using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class SegmentTests
{
    [TestCase(float.NaN, 0f, "startPoint")]
    [TestCase(0f, float.NaN, "startPoint")]
    [TestCase(float.PositiveInfinity, 0f, "startPoint")]
    [TestCase(0f, float.NegativeInfinity, "startPoint")]
    [TestCase(float.NaN, 0f, "endPoint")]
    [TestCase(0f, float.NaN, "endPoint")]
    [TestCase(float.PositiveInfinity, 0f, "endPoint")]
    [TestCase(0f, float.NegativeInfinity, "endPoint")]
    public void Constructor_WhenEndpointCoordinateIsInvalid_Throws(float x, float y, string paramName)
    {
        VectorXY startPoint = paramName == "startPoint" ? new VectorXY(x, y) : VectorXY.Zero;
        VectorXY endPoint = paramName == "endPoint" ? new VectorXY(x, y) : VectorXY.One;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Segment(startPoint, endPoint));

        Assert.That(exception!.ParamName, Is.EqualTo(paramName));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Shorten_WhenAmountIsInvalid_Throws(float amount)
    {
        var segment = new Segment(VectorXY.Zero, new VectorXY(10f, 0f));

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => segment.Shorten(amount));

        Assert.That(exception!.ParamName, Is.EqualTo("amount"));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Extend_WhenAmountIsInvalid_Throws(float amount)
    {
        var segment = new Segment(VectorXY.Zero, new VectorXY(10f, 0f));

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => segment.Extend(amount));

        Assert.That(exception!.ParamName, Is.EqualTo("amount"));
    }

    [Test]
    public void Shorten_WhenSegmentHasEndpointInclusion_PreservesEndpointInclusion()
    {
        var segment = new Segment(
            new VectorXY(0f, 0f),
            new VectorXY(10f, 0f),
            includesStartPoint: false,
            includesEndPoint: true);

        var shortened = segment.Shorten(1f);

        AssertVector(shortened.StartPoint, 1f, 0f);
        AssertVector(shortened.EndPoint, 9f, 0f);
        Assert.That(shortened.IncludesStartPoint, Is.False);
        Assert.That(shortened.IncludesEndPoint, Is.True);
    }

    [Test]
    public void Extend_WhenSegmentHasEndpointInclusion_PreservesEndpointInclusion()
    {
        var segment = new Segment(
            new VectorXY(0f, 0f),
            new VectorXY(10f, 0f),
            includesStartPoint: true,
            includesEndPoint: false);

        var extended = segment.Extend(1f);

        AssertVector(extended.StartPoint, -1f, 0f);
        AssertVector(extended.EndPoint, 11f, 0f);
        Assert.That(extended.IncludesStartPoint, Is.True);
        Assert.That(extended.IncludesEndPoint, Is.False);
    }

    [Test]
    public void BoundedParameterizedCurveContract_WhenSegmentIsUsed_ExposesEndpointsAndLength()
    {
        IBoundedParameterizedCurve curve = new Segment(new VectorXY(1f, 2f), new VectorXY(4f, 6f));

        AssertVector(curve.StartPoint, 1f, 2f);
        AssertVector(curve.EndPoint, 4f, 6f);
        Assert.That(curve.Length, Is.EqualTo(5f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void RayIntersections_WhenRayCrossesSegmentInterior_ReturnsIntersection()
    {
        var segment = new Segment(new VectorXY(1f, -1f), new VectorXY(1f, 1f));
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 1f, 0f);
    }

    [Test]
    public void RayIntersections_WhenEndpointIsExcluded_DoesNotReturnThatEndpoint()
    {
        var segment = new Segment(new VectorXY(1f, 0f), new VectorXY(1f, 1f), includesStartPoint: false, includesEndPoint: true);
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void RayIntersections_WhenEndpointIsIncluded_ReturnsThatEndpoint()
    {
        var segment = new Segment(new VectorXY(1f, 0f), new VectorXY(1f, 1f), includesStartPoint: true, includesEndPoint: true);
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 1f, 0f);
    }

    [Test]
    public void RayIntersections_WhenRayStartsInsideCollinearSegment_ReturnsRayOrigin()
    {
        var segment = new Segment(new VectorXY(0f, 0f), new VectorXY(10f, 0f));
        var ray = new Ray(new VectorXY(4f, 0f));

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 4f, 0f);
    }

    [Test]
    public void RayIntersections_WhenCollinearSegmentIsAhead_ReturnsFirstIncludedEndpoint()
    {
        var segment = new Segment(new VectorXY(4f, 0f), new VectorXY(10f, 0f));
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 4f, 0f);
    }

    [Test]
    public void RayIntersections_WithCustomGeometryEpsilon_WhenSegmentIsNearlyCollinear_ReturnsFirstEndpoint()
    {
        const float geometryEpsilon = 0.01f;
        ICurve segment = new Segment(new VectorXY(4f, 0.005f), new VectorXY(10f, 0.005f));
        var ray = new Ray(VectorXY.Zero);

        var defaultIntersections = segment.GetRayIntersections(ray);
        var tolerantIntersections = segment.GetRayIntersections(ray, geometryEpsilon);

        Assert.That(defaultIntersections, Is.Empty);
        Assert.That(tolerantIntersections, Has.Count.EqualTo(1));
        AssertVector(tolerantIntersections[0], 4f, 0.005f);
    }

    [Test]
    public void RayIntersections_WhenCollinearSegmentStartsAtExcludedRayOrigin_ReturnsEmpty()
    {
        var segment = new Segment(new VectorXY(0f, 0f), new VectorXY(10f, 0f), includesStartPoint: false, includesEndPoint: true);
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void ProjectWithParameter_WhenPointProjectsOutsideSegment_ClampsToNearestEndpoint()
    {
        var segment = new Segment(new VectorXY(2f, 0f), new VectorXY(4f, 0f));

        var projection = segment.ProjectWithParameter(VectorXY.Zero);

        AssertVector(projection.ProjectedPoint, 2f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(2f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void RayIntersections_WhenSegmentIsBehindRay_ReturnsEmpty()
    {
        var segment = new Segment(new VectorXY(-4f, 0f), new VectorXY(-2f, 0f));
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void RayIntersections_WhenDegenerateSegmentPointIsOnRay_ReturnsPoint()
    {
        var segment = new Segment(new VectorXY(2f, 0f), new VectorXY(2f, 0f));
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Has.Count.EqualTo(1));
        AssertVector(intersections[0], 2f, 0f);
    }

    [Test]
    public void RayIntersections_WhenDegenerateSegmentPointIsExcluded_ReturnsEmpty()
    {
        var segment = new Segment(new VectorXY(2f, 0f), new VectorXY(2f, 0f), includesStartPoint: false, includesEndPoint: false);
        var ray = new Ray(VectorXY.Zero);

        var intersections = segment.GetRayIntersections(ray);

        Assert.That(intersections, Is.Empty);
    }

    [Test]
    public void ProjectWithParameter_WhenPointProjectsInsideSegment_ReturnsInteriorProjection()
    {
        var segment = new Segment(new VectorXY(2f, 0f), new VectorXY(4f, 0f));

        var projection = segment.ProjectWithParameter(new VectorXY(3f, 2f));

        AssertVector(projection.ProjectedPoint, 3f, 0f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(1f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(2f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void ProjectWithParameter_WhenSegmentIsDegenerate_ReturnsEndpoint()
    {
        var segment = new Segment(new VectorXY(2f, 3f), new VectorXY(2f, 3f));

        var projection = segment.ProjectWithParameter(new VectorXY(5f, 7f));

        AssertVector(projection.ProjectedPoint, 2f, 3f);
        Assert.That(projection.CurveCoordinate, Is.EqualTo(0f).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(projection.Distance, Is.EqualTo(5f).Within(GeometryConstants.GeometryEpsilon));
    }

    [Test]
    public void ProjectWithParameter_WhenPointCoordinateIsInvalid_Throws()
    {
        var segment = new Segment(VectorXY.Zero, VectorXY.One);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            segment.ProjectWithParameter(new VectorXY(float.NaN, 0f)));

        Assert.That(exception!.ParamName, Is.EqualTo("point"));
    }

    [Test]
    public void Equals_WhenEndpointInclusionDiffers_ReturnsFalse()
    {
        var closed = new Segment(new VectorXY(1f, 0f), new VectorXY(1f, 1f), includesStartPoint: true, includesEndPoint: true);
        var openAtA = new Segment(new VectorXY(1f, 0f), new VectorXY(1f, 1f), includesStartPoint: false, includesEndPoint: true);

        Assert.That(closed, Is.Not.EqualTo(openAtA));
        Assert.That(closed.GetHashCode(), Is.Not.EqualTo(openAtA.GetHashCode()));
    }

    private static void AssertVector(VectorXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
