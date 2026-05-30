using Akeldov.Math.Hexes.Topology;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes
{
    public interface IHexMap<TValue>
    {
        HexFieldTopologySoA Topology { get; }

        int Width { get; }

        int Height { get; }

        Layout Layout { get; }

        TValue this[VectorXYInt index] { get; }

        TValue this[int index] { get; }
    }
}
