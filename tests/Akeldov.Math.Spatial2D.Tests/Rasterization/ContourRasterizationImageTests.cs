using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Contours.Rasterization;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Tests.Rasterization;

public class ContourRasterizationImageTests
{
    [Test]
    public void SaveAsBmp_WhenTriangleIsRasterizedToGray8Bit_WritesBmp8()
    {
        var contour = CreateTriangleContour();
        var grid = CreateTriangleGrid();
        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "triangle-gray8.bmp");

        contour
            .Rasterize(grid, new ContourSignedDistanceGray8BitRasterizer(ToGray8))
            .SaveAsBmp(path);

        Assert.That(File.Exists(path), Is.True);
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));

        byte[] bytes = File.ReadAllBytes(path);
        Assert.That(bytes[0..2], Is.EqualTo(new byte[] { 66, 77 }));
        Assert.That(BitConverter.ToUInt16(bytes, 28), Is.EqualTo(8));
    }

    [Test]
    public void SaveAsPng_WhenTriangleIsRasterizedToGray16Bit_WritesPng16()
    {
        var contour = CreateTriangleContour();
        var grid = CreateTriangleGrid();
        var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "triangle-gray16.png");

        contour
            .Rasterize(grid, new ContourSignedDistanceGray16BitRasterizer(ToGray16))
            .SaveAsPng(path);

        Assert.That(File.Exists(path), Is.True);
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));

        byte[] bytes = File.ReadAllBytes(path);
        Assert.That(bytes[0..8], Is.EqualTo(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }));
        Assert.That(bytes[24], Is.EqualTo(16));
        Assert.That(bytes[25], Is.EqualTo(0));
    }

    private static Contour CreateTriangleContour()
    {
        return new Contour(new IBoundedParameterizedCurve[]
        {
            new Segment(new VectorXY(0f, 0f), new VectorXY(4f, 0f)),
            new Segment(new VectorXY(4f, 0f), new VectorXY(2f, 3.5f)),
            new Segment(new VectorXY(2f, 3.5f), new VectorXY(0f, 0f))
        });
    }

    private static RasterGrid CreateTriangleGrid()
    {
        return new RasterGrid(
            origin: new VectorXY(-0.5f, -0.5f),
            size: new VectorXY(5f, 4.5f),
            resolution: new VectorXYInt(128, 128));
    }

    private static byte ToGray8(float signedDistance)
    {
        if (signedDistance <= 0f)
            return byte.MaxValue;

        const float falloffDistance = 0.2f;
        float normalized = 1f - System.Math.Clamp(signedDistance / falloffDistance, 0f, 1f);
        return (byte)System.MathF.Round(normalized * byte.MaxValue);
    }

    private static ushort ToGray16(float signedDistance)
    {
        if (signedDistance <= 0f)
            return ushort.MaxValue;

        const float falloffDistance = 0.2f;
        float normalized = 1f - System.Math.Clamp(signedDistance / falloffDistance, 0f, 1f);
        return (ushort)System.MathF.Round(normalized * ushort.MaxValue);
    }
}
