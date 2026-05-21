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

    [Test]
    public void LerpTo_WhenParameterIsNaN_Throws()
    {
        var source = new PointXY(1f, 2f);
        var target = new PointXY(5f, 10f);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            source.LerpTo(target, float.NaN));

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
