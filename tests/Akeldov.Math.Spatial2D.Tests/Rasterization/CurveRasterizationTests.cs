using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Tests.Rasterization;

public class CurveRasterizationTests
{
    [Test]
    public void Rasterize_WhenCurveIsLine_MapsDistanceToGray8()
    {
        ICurve curve = new Line(VectorXY.Zero, new VectorXY(1f, 0f));
        RasterGrid grid = CreateThreeByThreeGrid();

        Gray8BitRaster raster = curve.Rasterize(grid, new CurveDistanceGray8BitRasterizer(ToGray8));

        Assert.That(raster.Values[1, 0], Is.EqualTo(10));
        Assert.That(raster.Values[1, 1], Is.EqualTo(0));
        Assert.That(raster.Values[1, 2], Is.EqualTo(10));
    }

    [Test]
    public void Rasterize_WhenCurveCollectionIsProvided_MapsDistanceToNearestCurve()
    {
        IReadOnlyList<ICurve> curves = new ICurve[]
        {
            new Line(new VectorXY(0f, -1f), new VectorXY(1f, -1f)),
            new Line(new VectorXY(0f, 1f), new VectorXY(1f, 1f))
        };
        RasterGrid grid = CreateThreeByThreeGrid();

        Gray8BitRaster raster = curves.Rasterize(grid, new CurveCollectionDistanceGray8BitRasterizer(ToGray8));

        Assert.That(raster.Values[1, 0], Is.EqualTo(0));
        Assert.That(raster.Values[1, 1], Is.EqualTo(10));
        Assert.That(raster.Values[1, 2], Is.EqualTo(0));
    }

    [Test]
    public void Rasterize_WhenGridHasDefaultValue_Throws()
    {
        ICurve curve = new Line(VectorXY.Zero, new VectorXY(1f, 0f));
        IReadOnlyList<ICurve> curves = new ICurve[] { curve };

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            curve.Rasterize(default, new CurveDistanceGray8BitRasterizer(ToGray8)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            curves.Rasterize(default, new CurveCollectionDistanceGray8BitRasterizer(ToGray8)));
    }

    [Test]
    public void Rasterize_WhenCurveCollectionIsEmpty_Throws()
    {
        IReadOnlyList<ICurve> curves = Array.Empty<ICurve>();
        RasterGrid grid = CreateThreeByThreeGrid();

        var exception = Assert.Throws<ArgumentException>(() =>
            curves.Rasterize(grid, new CurveCollectionDistanceGray8BitRasterizer(ToGray8)));

        Assert.That(exception!.ParamName, Is.EqualTo("source"));
    }

    [Test]
    public void Rasterize_WhenCurveCollectionContainsNull_Throws()
    {
        IReadOnlyList<ICurve> curves = new ICurve[] { null! };
        RasterGrid grid = CreateThreeByThreeGrid();

        var exception = Assert.Throws<ArgumentException>(() =>
            curves.Rasterize(grid, new CurveCollectionDistanceGray8BitRasterizer(ToGray8)));

        Assert.That(exception!.ParamName, Is.EqualTo("source"));
    }

    private static RasterGrid CreateThreeByThreeGrid()
    {
        return new RasterGrid(
            origin: new VectorXY(-1.5f, -1.5f),
            size: new VectorXY(3f, 3f),
            resolution: new VectorXYInt(3, 3));
    }

    private static byte ToGray8(float distance)
    {
        return (byte)MathF.Round(distance * 10f);
    }
}
