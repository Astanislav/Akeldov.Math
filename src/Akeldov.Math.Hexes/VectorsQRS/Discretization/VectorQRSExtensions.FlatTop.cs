using System;

namespace Akeldov.Math.Hexes.Vectors.QRS.FlatTop
{
    public static partial class VectorQRSExtensions
    {
        public static VectorQRSInt ToQRSIndex(this VectorQRS axialPoint)
        {
            float s = -axialPoint.Q - axialPoint.R;

            int qInt = (int)MathF.Round(axialPoint.Q);
            int rInt = (int)MathF.Round(axialPoint.R);
            int sInt = (int)MathF.Round(s);

            float qDiff = MathF.Abs(qInt - axialPoint.Q);
            float rDiff = MathF.Abs(rInt - axialPoint.R);
            float sDiff = MathF.Abs(sInt - s);

            if (rDiff > qDiff && rDiff > sDiff)
                rInt = -qInt - sInt;
            else if (qDiff > sDiff)
                qInt = -rInt - sInt;

            return new VectorQRSInt(qInt, rInt);
        }
    }
}