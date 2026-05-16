using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Spatial2D.Tests.Imaging;

public class GrayRasterTests
{
    [Test]
    public void Gray8BitRaster_WhenSourceBufferChanges_ReflectsMutation()
    {
        byte[,] values = { { 1, 2 }, { 3, 4 } };
        var raster = new Gray8BitRaster(CreateGrid(), values);

        values[1, 0] = 9;

        Assert.That(raster.Values[1, 0], Is.EqualTo(9));
    }

    [Test]
    public void Gray8BitRasterClone_WhenCloneBufferChanges_DoesNotChangeSource()
    {
        byte[,] values = { { 1, 2 }, { 3, 4 } };
        var raster = new Gray8BitRaster(CreateGrid(), values);

        Gray8BitRaster clone = raster.Clone();
        clone.Values[1, 0] = 9;

        Assert.That(raster.Values[1, 0], Is.EqualTo(3));
        Assert.That(clone.Values[1, 0], Is.EqualTo(9));
    }

    [Test]
    public void Gray16BitRaster_WhenSourceBufferChanges_ReflectsMutation()
    {
        ushort[,] values = { { 1, 2 }, { 3, 4 } };
        var raster = new Gray16BitRaster(CreateGrid(), values);

        values[1, 0] = 9;

        Assert.That(raster.Values[1, 0], Is.EqualTo(9));
    }

    [Test]
    public void Gray16BitRasterClone_WhenCloneBufferChanges_DoesNotChangeSource()
    {
        ushort[,] values = { { 1, 2 }, { 3, 4 } };
        var raster = new Gray16BitRaster(CreateGrid(), values);

        Gray16BitRaster clone = raster.Clone();
        clone.Values[1, 0] = 9;

        Assert.That(raster.Values[1, 0], Is.EqualTo(3));
        Assert.That(clone.Values[1, 0], Is.EqualTo(9));
    }

    private static RasterGrid CreateGrid()
    {
        return new RasterGrid(VectorXY.Zero, VectorXY.One, new VectorXYInt(2, 2));
    }
}
