using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Imaging;

namespace Akeldov.Math.Hexes.Tests.Topology;

public class HexAdjacencyGridRGBA16BitRasterSnapshotTests
{
    [TestCase(Layout.OddR, "hex-adjacency-grid-odd-r-rgba16.png")]
    [TestCase(Layout.EvenR, "hex-adjacency-grid-even-r-rgba16.png")]
    [TestCase(Layout.OddQ, "hex-adjacency-grid-odd-q-rgba16.png")]
    [TestCase(Layout.EvenQ, "hex-adjacency-grid-even-q-rgba16.png")]
    public void ToRGBA16BitRaster_WithLayout_MatchesApprovedImage(Layout layout, string approvedFileName)
    {
        var adjacencyMap = new HexAdjacencyMap(
            width: 5,
            height: 4,
            layout: layout);
        var adjacencyGrid = new HexAdjacencyGrid(
            adjacencyMap,
            hexOrigin: VectorXY.Zero,
            hexApothem: 8f,
            resolution: new VectorXYInt(64, 64));
        RGBA16BitRaster raster = adjacencyGrid.ToRGBA16BitRaster(
            ToSnapshotColor,
            new RGBA16BitColor(0x1010, 0x1010, 0x1010, ushort.MaxValue));
        byte[] actual = SaveToPngBytes(raster, approvedFileName);

        AssertMatchesApprovedPng(approvedFileName, actual);
    }

    private static RGBA16BitColor ToSnapshotColor(int hexFlatIndex, HexAdjacency adjacency)
    {
        int adjacentCount = CountBits(adjacency.Flags);
        float red = 0.16f + 0.045f * hexFlatIndex;
        float green = 0.20f + 0.10f * adjacentCount;
        float blue = 0.72f - 0.035f * hexFlatIndex;

        return new RGBA16BitColor(
            ToChannel(red),
            ToChannel(green),
            ToChannel(blue),
            ushort.MaxValue);
    }

    private static int CountBits(HexAdjacencyFlags flags)
    {
        byte value = (byte)flags;
        int count = 0;

        for (int i = 0; i < 6; i++)
        {
            count += (value >> i) & 1;
        }

        return count;
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
            "Topology",
            "Approved",
            approvedFileName);

        if (!File.Exists(approvedPath))
        {
            string actualPath = GetActualPath(approvedFileName);
            TestContext.AddTestAttachment(actualPath, "Actual hex adjacency grid raster snapshot");
            Assert.Fail($"Hex adjacency grid approved image is missing. Actual image: {actualPath}");
        }

        byte[] approved = File.ReadAllBytes(approvedPath);

        if (!BytesEqual(actual, approved))
        {
            string actualPath = GetActualPath(approvedFileName);
            TestContext.AddTestAttachment(actualPath, "Actual hex adjacency grid raster snapshot");
            Assert.Fail($"Hex adjacency grid raster snapshot changed. Actual image: {actualPath}");
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
