using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Tests.Topology;

public class IndexedHexAdjacencyMapTests
{
    [Test]
    public void Constructor_ExposesDimensionsAndLayout()
    {
        var topology = new IndexedHexAdjacencyMap(3, 2, Layout.EvenQ);

        Assert.That(topology.Width, Is.EqualTo(3));
        Assert.That(topology.Height, Is.EqualTo(2));
        Assert.That(topology.Count, Is.EqualTo(6));
        Assert.That(topology.Layout, Is.EqualTo(Layout.EvenQ));
        Assert.That(topology.Adjacent, Has.Length.EqualTo(6));
    }

    [Test]
    public void IndexedHexAdjacencyMap_ImplementsIHexMap()
    {
        IHexMap<IndexedHexAdjacency> topology = new IndexedHexAdjacencyMap(3, 2, Layout.OddR);

        IndexedHexAdjacency adjacency = topology[new VectorXYInt(1, 0)];

        Assert.That(topology.Width, Is.EqualTo(3));
        Assert.That(topology.Height, Is.EqualTo(2));
        Assert.That(topology[1], Is.EqualTo(adjacency));
    }

    [Test]
    public void Constructor_CreatesIndexedAdjacency()
    {
        var topology = new IndexedHexAdjacencyMap(3, 2, Layout.OddR);

        IndexedHexAdjacency adjacency = topology[new VectorXYInt(1, 0)];

        Assert.That(adjacency.Index, Is.EqualTo(1));
        Assert.That(adjacency.Flags, Is.EqualTo(
            IndexedHexAdjacencyFlags.OwnIndex |
            IndexedHexAdjacencyFlags.Adjacent0 |
            IndexedHexAdjacencyFlags.Adjacent1 |
            IndexedHexAdjacencyFlags.Adjacent2 |
            IndexedHexAdjacencyFlags.Adjacent3));
        Assert.That(adjacency.HasOwnIndex, Is.True);
        Assert.That(adjacency.Adjacent0Index, Is.EqualTo(2));
        Assert.That(adjacency.Adjacent1Index, Is.EqualTo(4));
        Assert.That(adjacency.Adjacent2Index, Is.EqualTo(3));
        Assert.That(adjacency.Adjacent3Index, Is.EqualTo(0));
        Assert.That(adjacency.Adjacent4Index, Is.EqualTo(1));
        Assert.That(adjacency.Adjacent5Index, Is.EqualTo(1));
    }

    [Test]
    public void Indexer_WhenIndexIsOutsideTopology_Throws()
    {
        var topology = new IndexedHexAdjacencyMap(3, 2, Layout.OddR);

        Assert.Throws<IndexOutOfRangeException>(() => _ = topology[new VectorXYInt(3, 0)]);
        Assert.Throws<IndexOutOfRangeException>(() => _ = topology[new VectorXYInt(0, 2)]);
    }

    [Test]
    public void Constructor_WhenDimensionIsNegative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new IndexedHexAdjacencyMap(-1, 1, Layout.OddR));
        Assert.Throws<ArgumentOutOfRangeException>(() => new IndexedHexAdjacencyMap(1, -1, Layout.OddR));
    }
}
