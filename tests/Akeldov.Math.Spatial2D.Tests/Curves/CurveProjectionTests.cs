using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class CurveProjectionTests
{
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
}
