using System;

namespace Akeldov.Math.Hexes.Topology
{
    [Flags]
    public enum IndexedHexAdjacencyFlags : byte
    {
        None = 0,
        Adjacent0 = 1 << 0,
        Adjacent1 = 1 << 1,
        Adjacent2 = 1 << 2,
        Adjacent3 = 1 << 3,
        Adjacent4 = 1 << 4,
        Adjacent5 = 1 << 5,
        OwnIndex = 1 << 6,
        AllAdjacent = Adjacent0 | Adjacent1 | Adjacent2 | Adjacent3 | Adjacent4 | Adjacent5,
        All = OwnIndex | AllAdjacent
    }
}
