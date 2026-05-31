using Akeldov.Math.Hexes.Vectors.QRS;
using System.IO;

namespace Akeldov.Math.Hexes.Topology
{
    public static partial class BinaryReaderExtensions
    {
        public static Polyhex ReadPolyhexStamp(
            this BinaryReader binaryReader)
        {
            var isNotNull = binaryReader.ReadBoolean();
            if (isNotNull)
            {
                var dimension = binaryReader.ReadVectorQRSInt();
                var boolMask = binaryReader.ReadBoolMask();
                return new Polyhex(boolMask);
            }
            else
            {
                return null;
            }
        }
    }
}
