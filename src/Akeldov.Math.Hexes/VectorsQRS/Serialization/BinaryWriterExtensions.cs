using System.IO;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public static class BinaryWriterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter writer, VectorQRSInt vector)
        {
            writer.Write(vector.Q);
            writer.Write(vector.R);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter writer, VectorQRS vector)
        {
            writer.Write(vector.Q);
            writer.Write(vector.R);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this BinaryWriter writer, SixfoldAngle angle)
        {
            writer.Write((int)angle);
        }
    }
}
