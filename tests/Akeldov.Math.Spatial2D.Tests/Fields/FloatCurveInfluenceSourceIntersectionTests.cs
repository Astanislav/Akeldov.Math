using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class FloatCurveInfluenceSourceIntersectionTests
{
    [Test]
    public void GetRayIntersections_WithCustomGeometryEpsilon_PassesToleranceToUnderlyingCurve()
    {
        const float geometryEpsilon = 0.01f;
        var curve = new ParameterizedSegment(new PointXY(4f, 0.005f), new PointXY(10f, 0.005f));
        var source = new FloatCurveInfluenceSource(1f, curve, 0f);
        var ray = new Ray(new PointXY(0f, 0f));

        var defaultIntersections = source.GetRayIntersections(ray);
        var tolerantIntersections = source.GetRayIntersections(ray, geometryEpsilon);

        Assert.That(defaultIntersections, Is.Empty);
        Assert.That(tolerantIntersections, Has.Count.EqualTo(1));
        AssertVector(tolerantIntersections[0], 4f, 0.005f);
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void GetRayIntersections_WhenGeometryEpsilonIsInvalid_Throws(float geometryEpsilon)
    {
        var curve = new ParameterizedSegment(new PointXY(1f, -1f), new PointXY(1f, 1f));
        var source = new FloatCurveInfluenceSource(1f, curve, 0f);
        var ray = new Ray(new PointXY(0f, 0f));

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            source.GetRayIntersections(ray, geometryEpsilon));

        Assert.That(exception!.ParamName, Is.EqualTo("geometryEpsilon"));
    }

    private static void AssertVector(PointXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
