namespace Akeldov.Math.Hexes.Topology
{
    public class HexFieldTopology
    {
        private readonly int _width;
        private readonly int _height;

        public HexFieldTopology(
            int width,
            int height,
            byte[] hasAdjacent0,
            int[] adjacent0Index,
            int[] adjacent1Index,
            int[] adjacent2Index,
            int[] adjacent3Index,
            int[] adjacent4Index,
            int[] adjacent5Index)
        {
            _width = width;
            _height = height;
            HasAdjacent0 = hasAdjacent0;
            Adjacent0Index = adjacent0Index;
            Adjacent1Index = adjacent1Index;
            Adjacent2Index = adjacent2Index;
            Adjacent3Index = adjacent3Index;
            Adjacent4Index = adjacent4Index;
            Adjacent5Index = adjacent5Index;
        }

        public int Width => _width;
        public int Height => _height;
        public byte[] HasAdjacent0 { get; }
        public int[] Adjacent0Index { get; }
        public int[] Adjacent1Index { get; }
        public int[] Adjacent2Index { get; }
        public int[] Adjacent3Index { get; }
        public int[] Adjacent4Index { get; }
        public int[] Adjacent5Index { get; }
    }
}
