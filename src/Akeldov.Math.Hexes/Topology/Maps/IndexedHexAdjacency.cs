namespace Akeldov.Math.Hexes.Topology
{
    public readonly struct IndexedHexAdjacency
    {
        public IndexedHexAdjacency(
            IndexedHexAdjacencyFlags flags,
            int index,
            int adjacent0Index,
            int adjacent1Index,
            int adjacent2Index,
            int adjacent3Index,
            int adjacent4Index,
            int adjacent5Index)
        {
            Flags = flags & IndexedHexAdjacencyFlags.All;
            Index = index;
            Adjacent0Index = adjacent0Index;
            Adjacent1Index = adjacent1Index;
            Adjacent2Index = adjacent2Index;
            Adjacent3Index = adjacent3Index;
            Adjacent4Index = adjacent4Index;
            Adjacent5Index = adjacent5Index;
        }

        public IndexedHexAdjacencyFlags Flags { get; }

        public int Index { get; }

        public int Adjacent0Index { get; }

        public int Adjacent1Index { get; }

        public int Adjacent2Index { get; }

        public int Adjacent3Index { get; }

        public int Adjacent4Index { get; }

        public int Adjacent5Index { get; }

        public bool HasOwnIndex => (Flags & IndexedHexAdjacencyFlags.OwnIndex) != 0;

        public bool HasAdjacent0 => (Flags & IndexedHexAdjacencyFlags.Adjacent0) != 0;

        public bool HasAdjacent1 => (Flags & IndexedHexAdjacencyFlags.Adjacent1) != 0;

        public bool HasAdjacent2 => (Flags & IndexedHexAdjacencyFlags.Adjacent2) != 0;

        public bool HasAdjacent3 => (Flags & IndexedHexAdjacencyFlags.Adjacent3) != 0;

        public bool HasAdjacent4 => (Flags & IndexedHexAdjacencyFlags.Adjacent4) != 0;

        public bool HasAdjacent5 => (Flags & IndexedHexAdjacencyFlags.Adjacent5) != 0;
    }
}
