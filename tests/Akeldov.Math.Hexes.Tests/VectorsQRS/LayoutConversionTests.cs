using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Tests.VectorsQRS;

public class LayoutConversionTests
{
    [Test]
    public void XYAndQRSIndexConversions_RoundTrip_ForEveryLayout()
    {
        foreach (Layout layout in Enum.GetValues(typeof(Layout)))
        {
            for (int x = -3; x <= 3; x++)
            {
                for (int y = -3; y <= 3; y++)
                {
                    var xy = new VectorXYInt(x, y);

                    var qrs = xy.ToQRSIndex(layout);
                    var convertedBack = qrs.ToXYIndex(layout);

                    Assert.That(convertedBack, Is.EqualTo(xy), layout.ToString());
                }
            }
        }
    }

    [TestCase(Layout.OddR, 0, 1, 0, 1)]
    [TestCase(Layout.EvenR, 0, 1, -1, 1)]
    [TestCase(Layout.OddQ, 1, 0, 1, 0)]
    [TestCase(Layout.EvenQ, 1, 0, 1, -1)]
    [TestCase(Layout.OddR, 0, -1, 1, -1)]
    [TestCase(Layout.EvenR, 0, -1, 0, -1)]
    [TestCase(Layout.OddQ, -1, 0, -1, 1)]
    [TestCase(Layout.EvenQ, -1, 0, -1, 0)]
    public void ToQRSIndex_UsesLayoutOffset(Layout layout, int x, int y, int expectedQ, int expectedR)
    {
        var qrs = new VectorXYInt(x, y).ToQRSIndex(layout);

        Assert.That(qrs, Is.EqualTo(new VectorQRSInt(expectedQ, expectedR)));
    }

    [TestCase(Layout.OddR, 0, 1, 0, 1)]
    [TestCase(Layout.EvenR, 0, 1, 1, 1)]
    [TestCase(Layout.OddQ, 1, 0, 1, 0)]
    [TestCase(Layout.EvenQ, 1, 0, 1, 1)]
    [TestCase(Layout.OddR, 0, -1, -1, -1)]
    [TestCase(Layout.EvenR, 0, -1, 0, -1)]
    [TestCase(Layout.OddQ, -1, 0, -1, -1)]
    [TestCase(Layout.EvenQ, -1, 0, -1, 0)]
    public void ToXYIndex_UsesLayoutOffset(Layout layout, int q, int r, int expectedX, int expectedY)
    {
        var xy = new VectorQRSInt(q, r).ToXYIndex(layout);

        Assert.That(xy, Is.EqualTo(new VectorXYInt(expectedX, expectedY)));
    }

    [TestCase(Layout.OddR, 0f, 1f, 0, 1)]
    [TestCase(Layout.EvenR, 0f, 1f, 0, 1)]
    [TestCase(Layout.OddQ, 1f, 0f, 1, 0)]
    [TestCase(Layout.EvenQ, 1f, 0f, 1, 0)]
    public void FractionalToQRSIndex_DoesNotBakeOffsetIntoQRS(Layout layout, float q, float r, int expectedQ, int expectedR)
    {
        var qrs = new VectorQRS(q, r).ToQRSIndex(layout);

        Assert.That(qrs, Is.EqualTo(new VectorQRSInt(expectedQ, expectedR)));
    }
}
