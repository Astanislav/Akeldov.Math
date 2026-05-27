using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Hexes.Tests.VectorsQRS;

namespace Akeldov.Math.Hexes.Tests.Geometry.Fields;

public class HexFieldGeometryTests
{
    [Test]
    public void Constructor_UsesOriginAsZeroHexCenter_ForEveryLayout()
    {
        var origin = new VectorXY(10f, 20f);
        const float apothem = 2f;

        foreach (Layout layout in Enum.GetValues(typeof(Layout)))
        {
            var geometry = new HexFieldGeometry(2, 2, origin, apothem, layout);

            Assert.That(geometry.Width, Is.EqualTo(2));
            Assert.That(geometry.Height, Is.EqualTo(2));
            Assert.That(geometry.Origin, Is.EqualTo(origin));
            Assert.That(geometry.Apothem, Is.EqualTo(apothem));
            Assert.That(geometry.Layout, Is.EqualTo(layout));
            VectorAssert.AreEqual(geometry.Centers[0], origin.X, origin.Y);
        }
    }

    [TestCase(Layout.OddR, 2f, 2.3094f)]
    [TestCase(Layout.EvenR, 6f, 2.3094f)]
    [TestCase(Layout.OddQ, 2.3094f, 2f)]
    [TestCase(Layout.EvenQ, 2.3094f, 6f)]
    public void ToHexGeometrySoA_WithoutOrigin_PreservesDefaultZeroHexCenter(Layout layout, float expectedX, float expectedY)
    {
        var geometry = new VectorXYInt(1, 1).ToHexGeometrySoA(layout, 2f);

        VectorAssert.AreEqual(geometry.Centers[0], expectedX, expectedY);
    }
}
