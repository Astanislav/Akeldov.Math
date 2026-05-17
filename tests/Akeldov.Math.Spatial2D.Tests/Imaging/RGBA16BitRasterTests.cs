using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Tests.Imaging;

public class RGBA16BitRasterTests
{
    [Test]
    public void RGBA16BitRaster_WhenSourceBuffersChange_ReflectsMutation()
    {
        var redValues = new ushort[2, 3];
        var greenValues = new ushort[2, 3];
        var blueValues = new ushort[2, 3];
        var alphaValues = new ushort[2, 3];
        var raster = new RGBA16BitRaster(CreateGrid(), redValues, greenValues, blueValues, alphaValues);        

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
    public void RGBA16BitRasterClone_WhenCloneBuffersChange_DoesNotChangeSource()
    {
        var raster = new RGBA16BitRaster(
            CreateGrid(),
            new ushort[2, 3],
            new ushort[2, 3],
            new ushort[2, 3],
            new ushort[2, 3]);

        RGBA16BitRaster clone = raster.Clone();

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
    public void RGBA16BitRaster_WhenChannelDimensionsDoNotMatchGrid_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new RGBA16BitRaster(
                CreateGrid(),
                new ushort[1, 3],
                new ushort[2, 3],
                new ushort[2, 3],
                new ushort[2, 3]));
    }

    [Test]
    public void SaveAsPng_WhenRasterIsRGBA16Bit_WritesPng16WithAlpha()
    {
        var redValues = new ushort[2, 3];
        var greenValues = new ushort[2, 3];
        var blueValues = new ushort[2, 3];
        var alphaValues = new ushort[2, 3];
        redValues[0, 0] = 0x1234;
        greenValues[0, 0] = 0x5678;
        blueValues[0, 0] = 0x9abc;
        alphaValues[0, 0] = 0xdef0;
        var raster = new RGBA16BitRaster(CreateGrid(), redValues, greenValues, blueValues, alphaValues);
        string path = Path.Combine(TestContext.CurrentContext.WorkDirectory, "rgba16.png");

        raster.SaveAsPng(path);

        Assert.That(File.Exists(path), Is.True);
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));

        byte[] bytes = File.ReadAllBytes(path);
        Assert.That(bytes[0..8], Is.EqualTo(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }));
        Assert.That(bytes[24], Is.EqualTo(16));
        Assert.That(bytes[25], Is.EqualTo(6));
    }

    private static RasterGrid CreateGrid()
    {
        return new RasterGrid(VectorXY.Zero, new VectorXY(2f, 3f), new VectorXYInt(2, 3));
    }
}
