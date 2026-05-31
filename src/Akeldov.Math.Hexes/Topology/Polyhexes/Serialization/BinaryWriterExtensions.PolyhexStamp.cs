using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System.IO;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class BinaryWriterExtensions
    {
        public static void Write(
            this BinaryWriter binaryWriter,
            Polyhex polyhexStamp)
        {
            if (polyhexStamp != null)
            {
                binaryWriter.Write(true);
                binaryWriter.Write(polyhexStamp.Dimension);
                binaryWriter.Write(polyhexStamp.Mask);
            }
            else
            {
                binaryWriter.Write(false);
            }
        }
    }
}
