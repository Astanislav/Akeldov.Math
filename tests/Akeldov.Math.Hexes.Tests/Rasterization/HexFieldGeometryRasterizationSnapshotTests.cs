using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Rasterization;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Imaging;
using Akeldov.Math.Spatial2D.Rasterization;

namespace Akeldov.Math.Hexes.Tests.Rasterization;

public class HexFieldGeometryRasterizationSnapshotTests
{
    [TestCase(Layout.OddR, "hex-field-geometry-odd-r-rgba16.png")]
    [TestCase(Layout.EvenR, "hex-field-geometry-even-r-rgba16.png")]
    [TestCase(Layout.OddQ, "hex-field-geometry-odd-q-rgba16.png")]
    [TestCase(Layout.EvenQ, "hex-field-geometry-even-q-rgba16.png")]
    public void Rasterize_WithLayout_MatchesApprovedImage(Layout layout, string approvedFileName)
    {
        var geometry = new HexFieldGeometry(
            width: 5,
            height: 4,
            origin: new VectorXY(0f, 0f),
            apothem: 8f,
            layout: layout);
        RasterGrid grid = HexFieldGeometryRGBA16BitRasterizer.CreateGrid(geometry, pixelsPerApothem: 8f);
        RGBA16BitRaster raster = new HexFieldGeometryRGBA16BitRasterizer(ToSnapshotColor)
            .Rasterize(geometry, grid);
        byte[] actual = SaveToPngBytes(raster, approvedFileName);

        AssertMatchesApprovedPng(approvedFileName, actual);
    }

    private static RGBA16BitColor ToSnapshotColor(PointXY center)
    {
        float red = 0.22f + 0.04f * center.X;
        float green = 0.18f + 0.05f * center.Y;
        float blue = 0.72f - 0.006f * (center.X + center.Y);

        return new RGBA16BitColor(
            ToChannel(red),
            ToChannel(green),
            ToChannel(blue),
            ushort.MaxValue);
    }

    private static ushort ToChannel(float value)
    {
        value = MathF.Min(MathF.Max(value, 0f), 1f);
        return (ushort)MathF.Round(value * ushort.MaxValue);
    }

    private static byte[] SaveToPngBytes(RGBA16BitRaster raster, string approvedFileName)
    {
        string actualPath = GetActualPath(approvedFileName);
        raster.SaveAsPng(actualPath);
        return File.ReadAllBytes(actualPath);
    }

    private static void AssertMatchesApprovedPng(string approvedFileName, byte[] actual)
    {
        string approvedPath = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "Rasterization",
            "Approved",
            approvedFileName);

        if (!File.Exists(approvedPath))
        {
            string actualPath = GetActualPath(approvedFileName);
            TestContext.AddTestAttachment(actualPath, "Actual hex field geometry raster snapshot");
            Assert.Fail($"Hex field geometry approved image is missing. Actual image: {actualPath}");
        }

        byte[] approved = File.ReadAllBytes(approvedPath);

        if (!BytesEqual(actual, approved))
        {
            string actualPath = GetActualPath(approvedFileName);
            TestContext.AddTestAttachment(actualPath, "Actual hex field geometry raster snapshot");
            Assert.Fail($"Hex field geometry raster snapshot changed. Actual image: {actualPath}");
        }
    }

    private static string GetActualPath(string approvedFileName)
    {
        return Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            approvedFileName.Replace(".png", ".actual.png"));
    }

    private static bool BytesEqual(byte[] left, byte[] right)
    {
        if (left.Length != right.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
                return false;
        }

        return true;
    }
}
