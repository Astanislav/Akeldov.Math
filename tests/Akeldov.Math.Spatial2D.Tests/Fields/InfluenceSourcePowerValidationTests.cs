using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class InfluenceSourcePowerValidationTests
{
    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void FloatPointInfluenceSource_WhenPowerIsInvalid_Throws(float power)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FloatPointInfluenceSource(power, VectorXY.Zero, 1f));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void FloatPointInfluenceSource_WhenPowerIsAllowed_DoesNotThrow(float power)
    {
        Assert.DoesNotThrow(() =>
            new FloatPointInfluenceSource(power, VectorXY.Zero, 1f));
    }

    [Test]
    public void FloatPointInfluenceSource_WhenValueIsNaN_Throws()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FloatPointInfluenceSource(1f, VectorXY.Zero, float.NaN));

        Assert.That(exception!.ParamName, Is.EqualTo("value"));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void IntPointInfluenceSource_WhenPowerIsInvalid_Throws(float power)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new IntPointInfluenceSource(power, VectorXY.Zero, 1));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void IntPointInfluenceSource_WhenPowerIsAllowed_DoesNotThrow(float power)
    {
        Assert.DoesNotThrow(() =>
            new IntPointInfluenceSource(power, VectorXY.Zero, 1));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void FloatCurveInfluenceSource_WhenConstantPowerIsInvalid_Throws(float power)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FloatCurveInfluenceSource(power, curve, 1f));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void FloatCurveInfluenceSource_WhenConstantPowerIsAllowed_DoesNotThrow(float power)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);

        Assert.DoesNotThrow(() =>
            new FloatCurveInfluenceSource(power, curve, 1f));
    }

    [Test]
    public void FloatCurveInfluenceSource_WhenConstantValueIsNaN_Throws()
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FloatCurveInfluenceSource(1f, curve, float.NaN));

        Assert.That(exception!.ParamName, Is.EqualTo("value"));
    }

    [Test]
    public void FloatCurveInfluenceSource_WhenValueProviderReturnsNaN_Throws()
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);
        var source = new FloatCurveInfluenceSource(1f, curve, _ => float.NaN);

        Assert.Throws<InvalidOperationException>(() =>
            source.GetInfluence(VectorXY.Zero));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void FloatCurveInfluenceSource_WhenPowerProviderReturnsInvalidPower_Throws(float power)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);
        var source = new FloatCurveInfluenceSource(_ => power, curve, 1f);

        Assert.Throws<InvalidOperationException>(() =>
            source.GetInfluence(VectorXY.Zero));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void FloatCurveInfluenceSource_WhenPowerProviderReturnsAllowedPower_ReturnsPower(float power)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);
        var source = new FloatCurveInfluenceSource(_ => power, curve, 1f);

        var influence = source.GetInfluence(VectorXY.Zero);

        Assert.That(influence.Power, Is.EqualTo(power));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void InfluenceSample_WhenPowerIsInvalid_Throws(float power)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new InfluenceSample<float>(1f, VectorXY.Zero, 0f, power));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void InfluenceSample_WhenPowerIsAllowed_DoesNotThrow(float power)
    {
        Assert.DoesNotThrow(() =>
            new InfluenceSample<float>(1f, VectorXY.Zero, 0f, power));
    }
}
