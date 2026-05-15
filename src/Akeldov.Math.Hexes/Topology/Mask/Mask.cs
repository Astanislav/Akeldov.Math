using Akeldov.Math.Hexes.Vectors.QRS;
using System.Text;

namespace Akeldov.Math.Hexes.Topology
{
    public class Mask
    {
        private int _hash;

        public Mask(int[,] intMask) : this(intMask.ToBoolMask())
        {
        }

        public Mask(bool[,] boolMask)
        {
            BoolMask = boolMask;

            QSize = BoolMask.GetLength(0);
            RSize = BoolMask.GetLength(1);
            PositiveSize = BoolMask.PositiveSize();
            int hash = QSize ^ RSize;
            int r = 0;
            for (int i = 0; i < QSize; i++)
            {
                for (int j = 0; j < RSize; j++)
                {
                    hash = hash ^ ((BoolMask[i, j] ? 1 : 0) << r);
                    r = r + 1;
                }
            }
            _hash = hash;
        }

        public bool[,] BoolMask { get; }

        public int QSize { get; }

        public int RSize { get; }

        public int PositiveSize { get; }

        public bool this[VectorQRSInt index]
        {
            get => BoolMask[index.Q, index.R];
        }

        public bool this[int QIndex, int RIndex]
        {
            get => BoolMask[QIndex, RIndex];
        }

        public Mask GetExtended()
        {
            return new Mask(BoolMask.GetExtended());
        }

        public Mask GetContour()
        {
            return new Mask(BoolMask.GetContour());
        }

        public override int GetHashCode() => _hash;

        public override bool Equals(object obj) => obj is Mask other && Equals(other);

        public bool Equals(Mask other)
        {
            if (other is null)
                return false;

            if (QSize != other.QSize || RSize != other.RSize)
                return false;

            for (int i = 0; i < QSize; i++)
            {
                for (int j = 0; j < RSize; j++)
                {
                    if (BoolMask[i, j] != other.BoolMask[i, j])
                        return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < QSize; i++)
            {
                for (int j = 0; j < RSize; j++)
                {
                    sb.Append(BoolMask[i, j] ? 1 : 0);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


        public static implicit operator bool[,](Mask mask) => mask.BoolMask;


        public static implicit operator Mask(bool[,] boolMask) => new Mask(boolMask);

        public static bool operator ==(Mask left, Mask right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Mask left, Mask right) => !(left == right);
    }
}
