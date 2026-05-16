using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class ContourSmoothingExtensionsTests
{
    [Test]
    public void FilletCorners_WhenContourIsSegmentSquare_InsertsArcAtEachCorner()
    {
        var contour = new Contour(new IBoundedParameterizedCurve[]
        {
            new Segment(new VectorXY(0f, 0f), new VectorXY(2f, 0f)),
            new Segment(new VectorXY(2f, 0f), new VectorXY(2f, 2f)),
            new Segment(new VectorXY(2f, 2f), new VectorXY(0f, 2f)),
            new Segment(new VectorXY(0f, 2f), new VectorXY(0f, 0f))
        });

        Contour smoothed = contour.FilletCorners(0.25f);

        Assert.That(smoothed.Curves, Has.Count.EqualTo(8));
        Assert.That(smoothed.Curves.OfType<Segment>().Count(), Is.EqualTo(4));
        Assert.That(smoothed.Curves.OfType<Arc>().Count(), Is.EqualTo(4));
        Assert.That(smoothed.Curves.OfType<Arc>().Select(arc => arc.Radius), Is.All.EqualTo(0.25f));

        var firstSegment = (Segment)smoothed.Curves[0];
        AssertVector(firstSegment.StartPoint, 0.25f, 0f);
        AssertVector(firstSegment.EndPoint, 1.75f, 0f);
    }

    [Test]
    public void FilletCorners_WhenOnlySomeAdjacentCurvesAreSegments_InsertsArcsOnlyAtSegmentSegmentCorners()
    {
        var contour = new Contour(new IBoundedParameterizedCurve[]
        {
            new Segment(new VectorXY(0f, 0f), new VectorXY(2f, 0f)),
            new Arc(new VectorXY(2f, 1f), 1f, -0.5f * MathF.PI, 0.5f * MathF.PI),
            new Segment(new VectorXY(2f, 2f), new VectorXY(0f, 2f)),
            new Segment(new VectorXY(0f, 2f), new VectorXY(0f, 0f))
        });

        Contour smoothed = contour.FilletCorners(0.25f);

        Assert.That(smoothed.Curves, Has.Count.EqualTo(6));
        Assert.That(smoothed.Curves.OfType<Arc>().Count(), Is.EqualTo(3));
        Assert.That(smoothed.Curves.OfType<Arc>().Count(arc => arc.Radius.Equals(0.25f)), Is.EqualTo(2));
        Assert.That(smoothed.Curves[1], Is.TypeOf<Arc>());
    }

    [TestCase(0f)]
    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    public void FilletCorners_WhenRadiusIsInvalid_Throws(float radius)
    {
        var contour = new Contour(new IBoundedParameterizedCurve[]
        {
            new Segment(new VectorXY(0f, 0f), new VectorXY(1f, 0f)),
            new Segment(new VectorXY(1f, 0f), new VectorXY(0f, 1f)),
            new Segment(new VectorXY(0f, 1f), new VectorXY(0f, 0f))
        });

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => contour.FilletCorners(radius));

        Assert.That(exception!.ParamName, Is.EqualTo("radius"));
    }

    private static void AssertVector(VectorXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }
}
