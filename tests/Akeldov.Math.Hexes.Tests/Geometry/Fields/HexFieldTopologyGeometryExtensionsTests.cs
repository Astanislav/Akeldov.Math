using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Hexes.Tests.VectorsQRS;

namespace Akeldov.Math.Hexes.Tests.Geometry.Fields;

public class HexFieldTopologyGeometryExtensionsTests
{
    [Test]
    public void ToHexFieldGeometry_UsesTopologyDimensionsLayoutAndProvidedGeometry()
    {
        var topology = new HexFieldTopology(2, 1, Layout.OddR);
        var origin = new VectorXY(10f, 20f);

        HexFieldGeometry geometry = topology.ToHexFieldGeometry(origin, 2f);

        Assert.That(geometry.Width, Is.EqualTo(topology.Width));
        Assert.That(geometry.Height, Is.EqualTo(topology.Height));
        Assert.That(geometry.Layout, Is.EqualTo(topology.Layout));
        Assert.That(geometry.Origin, Is.EqualTo(origin));
        Assert.That(geometry.Apothem, Is.EqualTo(2f));
        VectorAssert.AreEqual(geometry.Centers[0], 10f, 20f);
        VectorAssert.AreEqual(geometry.Centers[1], 14f, 20f);
    }

    [Test]
    public void ToHexFieldGeometry_WhenTopologyIsNull_Throws()
    {
        HexFieldTopology topology = null!;

        Assert.Throws<ArgumentNullException>(() =>
            topology.ToHexFieldGeometry(new VectorXY(0f, 0f), 1f));
    }
}
