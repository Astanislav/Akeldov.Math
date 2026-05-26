namespace Akeldov.Math.Spatial2D.Tests.Points;

public class PointXYExtensionsTests
{
    [Test]
    public void LerpTo_WhenParameterIsBetweenZeroAndOne_ReturnsInterpolatedPoint()
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(5f, 10f);

        PointXY point = source.LerpTo(target, 0.25f);

        Assert.That(point, Is.EqualTo(new PointXY(2f, 4f)));
    }

    [TestCase(0f, 1f, 2f)]
    [TestCase(1f, 5f, 10f)]
    public void LerpTo_WhenParameterIsZeroOrOne_ReturnsEndpoint(
        float t,
        float expectedX,
        float expectedY)
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(5f, 10f);

        PointXY point = source.LerpTo(target, t);

        Assert.That(point, Is.EqualTo(new PointXY(expectedX, expectedY)));
    }

    [TestCase(-0.25f, 0f, 0f)]
    [TestCase(1.25f, 6f, 12f)]
    public void LerpTo_WhenParameterIsOutsideZeroToOne_ReturnsExtrapolatedPoint(
        float t,
        float expectedX,
        float expectedY)
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(5f, 10f);

        PointXY point = source.LerpTo(target, t);

        Assert.That(point, Is.EqualTo(new PointXY(expectedX, expectedY)));
    }

    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void LerpTo_WhenParameterIsInvalid_Throws(float t)
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(5f, 10f);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            source.LerpTo(target, t));

        Assert.That(exception!.ParamName, Is.EqualTo("t"));
    }

    [Test]
    public void SquaredDistanceTo_ReturnsSquaredDistanceBetweenPoints()
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(4f, 6f);

        Assert.That(source.SquaredDistanceTo(target), Is.EqualTo(25f));
    }

    [Test]
    public void AlmostEquals_WhenPointsAreWithinEuclideanDistanceTolerance_ReturnsTrue()
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(1f + GeometryConstants.GeometryEpsilon / 2f, 2f);

        Assert.That(source.AlmostEquals(target), Is.True);
    }

    [Test]
    public void AlmostEquals_WhenEuclideanDistanceExceedsTolerance_ReturnsFalse()
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(
            1f + GeometryConstants.GeometryEpsilon,
            2f + GeometryConstants.GeometryEpsilon);

        Assert.That(source.AlmostEquals(target), Is.False);
    }
}
