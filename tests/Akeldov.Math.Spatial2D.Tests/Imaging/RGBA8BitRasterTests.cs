using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Tests.Imaging;

public class RGBA8BitRasterTests
{
    [Test]
    public void RGBA8BitRaster_WhenSourceBuffersChange_ReflectsMutation()
    {
        var redValues = new byte[2, 3];
        var greenValues = new byte[2, 3];
        var blueValues = new byte[2, 3];
        var alphaValues = new byte[2, 3];
        var raster = new RGBA8BitRaster(CreateGrid(), redValues, greenValues, blueValues, alphaValues);

        redValues[1, 2] = 1;
        greenValues[1, 2] = 2;
        blueValues[1, 2] = 3;
        alphaValues[1, 2] = 4;

        Assert.That(raster.RedValues[1, 2], Is.EqualTo(1));
        Assert.That(raster.GreenValues[1, 2], Is.EqualTo(2));
        Assert.That(raster.BlueValues[1, 2], Is.EqualTo(3));
        Assert.That(raster.AlphaValues[1, 2], Is.EqualTo(4));
    }

    [Test]
    public void RGBA8BitRasterClone_WhenCloneBuffersChange_DoesNotChangeSource()
    {
        var raster = new RGBA8BitRaster(
            CreateGrid(),
            new byte[2, 3],
            new byte[2, 3],
            new byte[2, 3],
            new byte[2, 3]);

        RGBA8BitRaster clone = raster.Clone();

        clone.RedValues[1, 2] = 1;
        clone.GreenValues[1, 2] = 2;
        clone.BlueValues[1, 2] = 3;
        clone.AlphaValues[1, 2] = 4;

        Assert.That(raster.RedValues[1, 2], Is.EqualTo(0));
        Assert.That(raster.GreenValues[1, 2], Is.EqualTo(0));
        Assert.That(raster.BlueValues[1, 2], Is.EqualTo(0));
        Assert.That(raster.AlphaValues[1, 2], Is.EqualTo(0));
    }

    [Test]
    public void RGBA8BitRaster_WhenChannelDimensionsDoNotMatchGrid_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new RGBA8BitRaster(
                CreateGrid(),
                new byte[1, 3],
                new byte[2, 3],
                new byte[2, 3],
                new byte[2, 3]));
    }

    [Test]
    public void SaveAsPng_WhenRasterIsRGBA8Bit_WritesPng8WithAlpha()
    {
        RGBA8BitRaster raster = CreateRasterWithFirstPixel();
        string path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "rgba8.png");

        raster.SaveAsPng(path);

        Assert.That(File.Exists(path), Is.True);
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));

        byte[] bytes = File.ReadAllBytes(path);
        Assert.That(bytes[0..8], Is.EqualTo(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }));
        Assert.That(bytes[24], Is.EqualTo(8));
        Assert.That(bytes[25], Is.EqualTo(6));
    }

    [Test]
    public void SaveAsBmp_WhenRasterIsRGBA8Bit_WritesBmp32WithBgraPixels()
    {
        RGBA8BitRaster raster = CreateRasterWithFirstPixel();
        string path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "rgba8.bmp");

        raster.SaveAsBmp(path);

        Assert.That(File.Exists(path), Is.True);
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));

        byte[] bytes = File.ReadAllBytes(path);
        int pixelOffset = BitConverter.ToInt32(bytes, 10);
        short bitsPerPixel = BitConverter.ToInt16(bytes, 28);

        Assert.That(bytes[0], Is.EqualTo((byte)'B'));
        Assert.That(bytes[1], Is.EqualTo((byte)'M'));
        Assert.That(bitsPerPixel, Is.EqualTo(32));
        Assert.That(bytes[pixelOffset], Is.EqualTo(0x56));
        Assert.That(bytes[pixelOffset + 1], Is.EqualTo(0x34));
        Assert.That(bytes[pixelOffset + 2], Is.EqualTo(0x12));
        Assert.That(bytes[pixelOffset + 3], Is.EqualTo(0x78));
    }

    private static RGBA8BitRaster CreateRasterWithFirstPixel()
    {
        var redValues = new byte[2, 3];
        var greenValues = new byte[2, 3];
        var blueValues = new byte[2, 3];
        var alphaValues = new byte[2, 3];
        redValues[0, 0] = 0x12;
        greenValues[0, 0] = 0x34;
        blueValues[0, 0] = 0x56;
        alphaValues[0, 0] = 0x78;

        return new RGBA8BitRaster(CreateGrid(), redValues, greenValues, blueValues, alphaValues);
    }

    private static RasterGrid CreateGrid()
    {
        return new RasterGrid(VectorXY.Zero, new VectorXY(2f, 3f), new VectorXYInt(2, 3));
    }
}
