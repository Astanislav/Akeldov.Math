using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;
using System.IO;
using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Write(this BinaryWriter writer, bool[,] mask)
        {
            if (mask != null)
            {
                writer.Write(true);
                var dim = new VectorXYInt(mask.GetLength(0), mask.GetLength(1));
                writer.Write(dim);
                for (int i = 0; i < dim.X; i++)
                {
                    for (int j = 0; j < dim.X; j++)
                    {
                        writer.Write(mask[i, j]);
                    }
                }
            }
            else
            {
                writer.Write(false);
            }
        }
    }
}
