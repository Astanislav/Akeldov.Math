using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Contours.Rasterization;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Tests.Rasterization;

public class RegionRasterizationTests
{
    [Test]
    public void Rasterize_WhenRegionHasHole_UsesRegionFillRule()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });
        var grid = new RasterGrid(
            origin: VectorXY.Zero,
            size: new VectorXY(4f, 4f),
            resolution: new VectorXYInt(4, 4));

        Gray8BitRaster raster = region.Rasterize(grid, new RegionSignedDistanceGray8BitRasterizer(ToMaskValue));

        Assert.That(raster.Values[0, 0], Is.EqualTo(byte.MaxValue));
        Assert.That(raster.Values[1, 1], Is.EqualTo(byte.MinValue));
        Assert.That(raster.Values[2, 2], Is.EqualTo(byte.MinValue));
        Assert.That(raster.Values[3, 3], Is.EqualTo(byte.MaxValue));
    }

    [Test]
    public void Rasterize_WhenGridHasDefaultValue_Throws()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f)
        });

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            region.Rasterize(default, new RegionSignedDistanceGray8BitRasterizer(ToMaskValue)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            region.Rasterize(default, new RegionSignedDistanceGray16BitRasterizer(ToGray16)));
    }

    private static byte ToMaskValue(float signedDistance)
    {
        return signedDistance <= 0f ? byte.MaxValue : byte.MinValue;
    }

    private static ushort ToGray16(float signedDistance)
    {
        return signedDistance <= 0f ? ushort.MaxValue : ushort.MinValue;
    }

    private static Contour CreateSquareContour(float left, float bottom, float right, float top)
    {
        return new Contour(new IBoundedParameterizedCurve[]
        {
            new Segment(new VectorXY(left, bottom), new VectorXY(right, bottom)),
            new Segment(new VectorXY(right, bottom), new VectorXY(right, top)),
            new Segment(new VectorXY(right, top), new VectorXY(left, top)),
            new Segment(new VectorXY(left, top), new VectorXY(left, bottom))
        });
    }
}
