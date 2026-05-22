using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;

namespace Akeldov.Math.Spatial2D.Tests.Rasterization;

public class PoissonDiskPointSampleRasterizationTests
{
    [Test]
    public void Rasterize_WhenPoissonDiskSamplesAreProvided_MapsNearestSampleAndDistanceToColor()
    {
        var samples = new[]
        {
            new PoissonDiskPointSample(new PointXY(0.5f, 0.5f), 1f),
            new PoissonDiskPointSample(new PointXY(2.5f, 0.5f), 2f)
        };
        var grid = new RasterGrid(
            origin: new PointXY(0f, 0f),
            size: new VectorXY(3f, 1f),
            resolution: new VectorXYInt(3, 1));

        RGBA16BitRaster raster = samples.Rasterize(grid, ToColor);

        Assert.That(raster[0, 0], Is.EqualTo(new RGBA16BitColor(1000, 0, 0, ushort.MaxValue)));
        Assert.That(raster[1, 0], Is.EqualTo(new RGBA16BitColor(1000, 1000, 0, ushort.MaxValue)));
        Assert.That(raster[2, 0], Is.EqualTo(new RGBA16BitColor(2000, 0, 0, ushort.MaxValue)));
    }

    [Test]
    public void Constructor_WhenColorMapperIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PoissonDiskPointSampleCollectionDistanceRGBA16BitRasterizer(null!));
    }

    [Test]
    public void Rasterize_WhenSourceIsEmpty_Throws()
    {
        var rasterizer = new PoissonDiskPointSampleCollectionDistanceRGBA16BitRasterizer(ToColor);

        var exception = Assert.Throws<ArgumentException>(() =>
            rasterizer.Rasterize(Array.Empty<PoissonDiskPointSample>(), CreateGrid()));

        Assert.That(exception!.ParamName, Is.EqualTo("source"));
    }

    [Test]
    public void Rasterize_WhenSourceContainsDefaultSample_Throws()
    {
        var rasterizer = new PoissonDiskPointSampleCollectionDistanceRGBA16BitRasterizer(ToColor);
        var samples = new[] { default(PoissonDiskPointSample) };

        var exception = Assert.Throws<ArgumentException>(() =>
            rasterizer.Rasterize(samples, CreateGrid()));

        Assert.That(exception!.ParamName, Is.EqualTo("source"));
    }

    [Test]
    public void Rasterize_WhenGridHasDefaultValue_Throws()
    {
        var rasterizer = new PoissonDiskPointSampleCollectionDistanceRGBA16BitRasterizer(ToColor);
        var samples = new[] { new PoissonDiskPointSample(new PointXY(0f, 0f), 1f) };

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            rasterizer.Rasterize(samples, default));
    }

    private static RasterGrid CreateGrid()
    {
        return new RasterGrid(new PointXY(0f, 0f), new VectorXY(1f, 1f), new VectorXYInt(1, 1));
    }

    private static RGBA16BitColor ToColor(PoissonDiskPointSample sample, float distance)
    {
        return new RGBA16BitColor(
            (ushort)MathF.Round(sample.MinimalDistance * 1000f),
            (ushort)MathF.Round(distance * 1000f),
            0,
            ushort.MaxValue);
    }
}
