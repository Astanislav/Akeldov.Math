using System;

namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public static partial class VectorQRSExtensions
    {
        public static VectorQRSInt ToQRSIndex(this VectorQRS axialPoint, Layout layout)
        {
            switch (layout)
            {
                case Layout.OddR:
                    {
                        float s = -axialPoint.Q - axialPoint.R;

                        int qInt = (int)MathF.Round(axialPoint.Q);
                        int rInt = (int)MathF.Round(axialPoint.R);
                        int sInt = (int)MathF.Round(s);

                        float qDiff = MathF.Abs(qInt - axialPoint.Q);
                        float rDiff = MathF.Abs(rInt - axialPoint.R);
                        float sDiff = MathF.Abs(sInt - s);

                        if (qDiff > rDiff && qDiff > sDiff)
                            qInt = -rInt - sInt;
                        else if (rDiff > sDiff)
                            rInt = -qInt - sInt;

                        return new VectorQRSInt(qInt, rInt);
                    }
                case Layout.EvenR:
                    {
                        float s = -axialPoint.Q - axialPoint.R;

                        int qInt = (int)MathF.Round(axialPoint.Q);
                        int rInt = (int)MathF.Round(axialPoint.R);
                        int sInt = (int)MathF.Round(s);

                        float qDiff = MathF.Abs(qInt - axialPoint.Q);
                        float rDiff = MathF.Abs(rInt - axialPoint.R);
                        float sDiff = MathF.Abs(sInt - s);

                        if (qDiff > rDiff && qDiff > sDiff)
                            qInt = -rInt - sInt;
                        else if (rDiff > sDiff)
                            rInt = -qInt - sInt;

                        var shift = rInt % 2 ==  0 ? 0 : 1;

                        return new VectorQRSInt(qInt + shift, rInt);
                    }
                case Layout.OddQ:
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
                case Layout.EvenQ:
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

                        var shift = qInt % 2 == 0 ? 0 : 1;

                        return new VectorQRSInt(qInt, rInt + shift);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }
    }
}