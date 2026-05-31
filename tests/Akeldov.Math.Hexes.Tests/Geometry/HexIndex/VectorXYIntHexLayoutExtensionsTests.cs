using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Tests.VectorsQRS;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Tests.Geometry.HexIndex;

public class VectorXYIntHexLayoutExtensionsTests
{
    [Test]
    public void GetHexCenter_WithOrigin_UsesOriginAsZeroHexCenter_ForEveryLayout()
    {
        var origin = new VectorXY(10f, 20f);
        const float apothem = 2f;
        float radius = apothem.ConvertHexApothemToRadius();

        foreach (Layout layout in Enum.GetValues(typeof(Layout)))
        {
            VectorXY center = VectorXYInt.Zero.GetHexCenter(apothem, radius, origin, layout);

            VectorAssert.AreEqual(center, origin.X, origin.Y);
        }
    }

    [TestCase(Layout.OddR, 0, 1, 12f, 23.4641f)]
    [TestCase(Layout.EvenR, 0, 1, 8f, 23.4641f)]
    [TestCase(Layout.OddQ, 1, 0, 13.4641f, 22f)]
    [TestCase(Layout.EvenQ, 1, 0, 13.4641f, 18f)]
    public void GetHexCenter_WithOrigin_OffsetsShiftedAxesRelativeToZeroHexCenter(
        Layout layout,
        int x,
        int y,
        float expectedX,
        float expectedY)
    {
        var origin = new VectorXY(10f, 20f);
        const float apothem = 2f;
        float radius = apothem.ConvertHexApothemToRadius();

        VectorXY center = new VectorXYInt(x, y).GetHexCenter(apothem, radius, origin, layout);

        VectorAssert.AreEqual(center, expectedX, expectedY);
    }

    [TestCase(Layout.OddR, 2f, 2.3094f)]
    [TestCase(Layout.EvenR, 6f, 2.3094f)]
    [TestCase(Layout.OddQ, 2.3094f, 2f)]
    [TestCase(Layout.EvenQ, 2.3094f, 6f)]
    public void GetHexCenter_WithoutOrigin_PreservesDefaultZeroHexCenter(
        Layout layout,
        float expectedX,
        float expectedY)
    {
        const float apothem = 2f;
        float radius = apothem.ConvertHexApothemToRadius();

        VectorXY center = VectorXYInt.Zero.GetHexCenter(apothem, radius, layout);

        VectorAssert.AreEqual(center, expectedX, expectedY);
    }
}
