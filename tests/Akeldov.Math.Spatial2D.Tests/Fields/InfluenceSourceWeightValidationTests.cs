using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class InfluenceSourceWeightValidationTests
{
    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void FloatPointInfluenceSource_WhenWeightIsInvalid_Throws(float weight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FloatPointInfluenceSource(weight, VectorXY.Zero, 1f));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void FloatPointInfluenceSource_WhenWeightIsAllowed_DoesNotThrow(float weight)
    {
        Assert.DoesNotThrow(() =>
            new FloatPointInfluenceSource(weight, VectorXY.Zero, 1f));
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
    public void IntPointInfluenceSource_WhenWeightIsInvalid_Throws(float weight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new IntPointInfluenceSource(weight, VectorXY.Zero, 1));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void IntPointInfluenceSource_WhenWeightIsAllowed_DoesNotThrow(float weight)
    {
        Assert.DoesNotThrow(() =>
            new IntPointInfluenceSource(weight, VectorXY.Zero, 1));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void FloatCurveInfluenceSource_WhenConstantWeightIsInvalid_Throws(float weight)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FloatCurveInfluenceSource(weight, curve, 1f));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void FloatCurveInfluenceSource_WhenConstantWeightIsAllowed_DoesNotThrow(float weight)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);

        Assert.DoesNotThrow(() =>
            new FloatCurveInfluenceSource(weight, curve, 1f));
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
    public void FloatCurveInfluenceSource_WhenWeightProviderReturnsInvalidWeight_Throws(float weight)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);
        var source = new FloatCurveInfluenceSource(_ => weight, curve, 1f);

        Assert.Throws<InvalidOperationException>(() =>
            source.GetInfluence(VectorXY.Zero));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void FloatCurveInfluenceSource_WhenWeightProviderReturnsAllowedWeight_ReturnsWeight(float weight)
    {
        var curve = new Segment(VectorXY.Zero, VectorXY.One);
        var source = new FloatCurveInfluenceSource(_ => weight, curve, 1f);

        var influence = source.GetInfluence(VectorXY.Zero);

        Assert.That(influence.Weight, Is.EqualTo(weight));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.NegativeInfinity)]
    public void InfluenceSample_WhenWeightIsInvalid_Throws(float weight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new InfluenceSample<float>(1f, VectorXY.Zero, 0f, weight));
    }

    [TestCase(0f)]
    [TestCase(float.PositiveInfinity)]
    public void InfluenceSample_WhenWeightIsAllowed_DoesNotThrow(float weight)
    {
        Assert.DoesNotThrow(() =>
            new InfluenceSample<float>(1f, VectorXY.Zero, 0f, weight));
    }

    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void InfluenceSample_WhenDistanceIsInvalid_Throws(float distance)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new InfluenceSample<float>(1f, VectorXY.Zero, distance, 1f));

        Assert.That(exception!.ParamName, Is.EqualTo("distance"));
    }

    [TestCase(0f)]
    [TestCase(1f)]
    public void InfluenceSample_WhenDistanceIsAllowed_DoesNotThrow(float distance)
    {
        Assert.DoesNotThrow(() =>
            new InfluenceSample<float>(1f, VectorXY.Zero, distance, 1f));
    }

    [Test]
    public void InfluenceSample_WhenCreated_StoresSourcePoint()
    {
        var sourcePoint = new VectorXY(2f, 3f);

        var sample = new InfluenceSample<float>(1f, sourcePoint, 4f, 5f);

        Assert.That(sample.SourcePoint, Is.EqualTo(sourcePoint));
    }
}
