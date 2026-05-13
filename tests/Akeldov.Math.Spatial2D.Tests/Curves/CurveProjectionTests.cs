using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class CurveProjectionTests
{
    [TestCase(float.NaN, 0f)]
    [TestCase(0f, float.NaN)]
    [TestCase(float.PositiveInfinity, 0f)]
    [TestCase(0f, float.NegativeInfinity)]
    public void CurveProjectionConstructor_WhenProjectedPointIsInvalid_Throws(float x, float y)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CurveProjection(new VectorXY(x, y), 0f));

        Assert.That(exception!.ParamName, Is.EqualTo("projectedPoint"));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void CurveProjectionConstructor_WhenDistanceIsInvalid_Throws(float distance)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CurveProjection(VectorXY.Zero, distance));

        Assert.That(exception!.ParamName, Is.EqualTo("distance"));
    }

    [TestCase(float.NaN, 0f)]
    [TestCase(0f, float.NaN)]
    [TestCase(float.PositiveInfinity, 0f)]
    [TestCase(0f, float.NegativeInfinity)]
    public void ParameterizedCurveProjectionConstructor_WhenProjectedPointIsInvalid_Throws(float x, float y)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ParameterizedCurveProjection(new VectorXY(x, y), 0f, 0f));

        Assert.That(exception!.ParamName, Is.EqualTo("projectedPoint"));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void ParameterizedCurveProjectionConstructor_WhenDistanceIsInvalid_Throws(float distance)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ParameterizedCurveProjection(VectorXY.Zero, 0f, distance));

        Assert.That(exception!.ParamName, Is.EqualTo("distance"));
    }

    [Test]
    public void ParameterizedCurveProjectionConstructor_WhenCurveCoordinateIsNaN_Throws()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ParameterizedCurveProjection(VectorXY.Zero, float.NaN, 0f));

        Assert.That(exception!.ParamName, Is.EqualTo("curveCoordinate"));
    }

    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void ParameterizedCurveProjectionConstructor_WhenCurveCoordinateIsInfinity_DoesNotThrow(float curveCoordinate)
    {
        Assert.DoesNotThrow(() =>
            new ParameterizedCurveProjection(VectorXY.Zero, curveCoordinate, 0f));
    }
}
