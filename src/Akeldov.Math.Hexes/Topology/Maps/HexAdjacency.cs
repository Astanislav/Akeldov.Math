namespace Akeldov.Math.Hexes.Topology
{
    public readonly struct HexAdjacency
    {
        public HexAdjacency(
            HexAdjacencyFlags flags,
            int adjacent0Index,
            int adjacent1Index,
            int adjacent2Index,
            int adjacent3Index,
            int adjacent4Index,
            int adjacent5Index)
        {
            Flags = flags;
            Adjacent0Index = adjacent0Index;
            Adjacent1Index = adjacent1Index;
            Adjacent2Index = adjacent2Index;
            Adjacent3Index = adjacent3Index;
            Adjacent4Index = adjacent4Index;
            Adjacent5Index = adjacent5Index;
        }

        public HexAdjacencyFlags Flags { get; }

        public int Adjacent0Index { get; }

        public int Adjacent1Index { get; }

        public int Adjacent2Index { get; }

        public int Adjacent3Index { get; }

        public int Adjacent4Index { get; }

        public int Adjacent5Index { get; }

        public bool HasAdjacent0 => (Flags & HexAdjacencyFlags.Adjacent0) != 0;

        public bool HasAdjacent1 => (Flags & HexAdjacencyFlags.Adjacent1) != 0;

        public bool HasAdjacent2 => (Flags & HexAdjacencyFlags.Adjacent2) != 0;

        public bool HasAdjacent3 => (Flags & HexAdjacencyFlags.Adjacent3) != 0;

        public bool HasAdjacent4 => (Flags & HexAdjacencyFlags.Adjacent4) != 0;

        public bool HasAdjacent5 => (Flags & HexAdjacencyFlags.Adjacent5) != 0;
    }
}
