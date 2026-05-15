using Akeldov.Math.Hexes.Vectors.QRS;

namespace Akeldov.Math.Hexes.Topology
{
    public interface IPolyhex
    {
        VectorQRSInt Dimension { get; }

        Mask Mask { get; }
    }
}
