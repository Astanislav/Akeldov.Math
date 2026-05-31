using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Tests.Topology;

public class HexFieldTopologyTests
{
    [Test]
    public void Constructor_ExposesDimensionsAndLayout()
    {
        var topology = new HexAdjacencyMap(3, 2, Layout.EvenQ);

        Assert.That(topology.Width, Is.EqualTo(3));
        Assert.That(topology.Height, Is.EqualTo(2));
        Assert.That(topology.Count, Is.EqualTo(6));
        Assert.That(topology.Layout, Is.EqualTo(Layout.EvenQ));
        Assert.That(topology.Adjacent, Has.Length.EqualTo(6));
    }

    [Test]
    public void HexFieldTopology_ImplementsIHexMap()
    {
        IHexMap<HexAdjacency> topology = new HexAdjacencyMap(3, 2, Layout.OddR);

        HexAdjacency adjacency = topology[new VectorXYInt(1, 0)];

        Assert.That(topology.Width, Is.EqualTo(3));
        Assert.That(topology.Height, Is.EqualTo(2));
        Assert.That(topology[1], Is.EqualTo(adjacency));
    }

    [Test]
    public void Constructor_FromSoA_PreservesAdjacency()
    {
        var source = new HexFieldTopologySoA(3, 2, Layout.OddR);

        var topology = new HexAdjacencyMap(source);
        HexAdjacency adjacency = topology[new VectorXYInt(1, 0)];

        Assert.That(adjacency.HasAdjacent, Is.EqualTo(source.HasAdjacent[1]));
        Assert.That(adjacency.Adjacent0Index, Is.EqualTo(source.Adjacent0Index[1]));
        Assert.That(adjacency.Adjacent1Index, Is.EqualTo(source.Adjacent1Index[1]));
        Assert.That(adjacency.Adjacent2Index, Is.EqualTo(source.Adjacent2Index[1]));
        Assert.That(adjacency.Adjacent3Index, Is.EqualTo(source.Adjacent3Index[1]));
        Assert.That(adjacency.Adjacent4Index, Is.EqualTo(source.Adjacent4Index[1]));
        Assert.That(adjacency.Adjacent5Index, Is.EqualTo(source.Adjacent5Index[1]));
    }

    [Test]
    public void Indexer_WhenIndexIsOutsideTopology_Throws()
    {
        var topology = new HexAdjacencyMap(3, 2, Layout.OddR);

        Assert.Throws<IndexOutOfRangeException>(() => _ = topology[new VectorXYInt(3, 0)]);
        Assert.Throws<IndexOutOfRangeException>(() => _ = topology[new VectorXYInt(0, 2)]);
    }

    [Test]
    public void Constructor_WhenSourceIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new HexAdjacencyMap(null!));
    }
}
