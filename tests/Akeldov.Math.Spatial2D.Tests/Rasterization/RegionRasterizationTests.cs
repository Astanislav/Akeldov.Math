using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;
using Akeldov.Math.Spatial2D.Regions;

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

    [Test]
    public void SaveAsPng_WhenSquareWithSquareHoleIsRasterizedToGray16Bit_WritesPng16()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });
        var grid = new RasterGrid(
            origin: new VectorXY(-0.5f, -0.5f),
            size: new VectorXY(5f, 5f),
            resolution: new VectorXYInt(160, 160));
        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "square-with-square-hole-gray16.png");

        region
            .Rasterize(grid, new RegionSignedDistanceGray16BitRasterizer(ToDistanceGray16))
            .SaveAsPng(path);

        Assert.That(File.Exists(path), Is.True);
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));

        byte[] bytes = File.ReadAllBytes(path);
        Assert.That(bytes[0..8], Is.EqualTo(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }));
        Assert.That(bytes[24], Is.EqualTo(16));
        Assert.That(bytes[25], Is.EqualTo(0));
    }

    private static byte ToMaskValue(float signedDistance)
    {
        return signedDistance <= 0f ? byte.MaxValue : byte.MinValue;
    }

    private static ushort ToGray16(float signedDistance)
    {
        return signedDistance <= 0f ? ushort.MaxValue : ushort.MinValue;
    }

    private static ushort ToDistanceGray16(float signedDistance)
    {
        if (signedDistance <= 0f)
            return ushort.MaxValue;

        const float falloffDistance = 0.2f;
        float normalized = 1f - System.Math.Clamp(signedDistance / falloffDistance, 0f, 1f);
        return (ushort)System.MathF.Round(normalized * ushort.MaxValue);
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
