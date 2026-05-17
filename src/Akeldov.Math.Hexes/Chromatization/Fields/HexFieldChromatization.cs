namespace Akeldov.Math.Hexes.Chromatization
{
    public sealed class HexFieldChromatization
    {
        private readonly int _width;
        private readonly int _height;

        public HexFieldChromatization(int width, int height, byte[] chromaticIndices)
        {
            _width = width;
            _height = height;
            ChromaticIndices = chromaticIndices;
        }

        public int Width => _width;

        public int Height => _height;

        public byte[] ChromaticIndices { get; }
    }
}
