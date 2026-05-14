using Akeldov.Math.Spatial2D;
using System;
using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Vectors.QRS
{
    public static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ToXYIndexOddR(in VectorXY point, float hexRadius, in VectorXY hexFieldOrigin)
        {
            
            float shiftedX = point.X - hexFieldOrigin.X;
            float shiftedY = point.Y - hexFieldOrigin.Y;

            
            float qf = 0.5773502588f * shiftedX - 0.3333333333f * shiftedY;
            float rf = 0.6666666666f * shiftedY;

            
            if (hexRadius == 0f)
                throw new ArgumentOutOfRangeException(nameof(hexRadius), hexRadius, "hexRadius cannot be zero");

            
            float qNorm = qf / hexRadius;
            float rNorm = rf / hexRadius;
            float sNorm = -qNorm - rNorm;

            
            int qInt = (int)MathF.Round(qNorm);
            int rInt = (int)MathF.Round(rNorm);
            int sInt = (int)MathF.Round(sNorm);

            
            float qDiff = MathF.Abs(qInt - qNorm);
            float rDiff = MathF.Abs(rInt - rNorm);
            float sDiff = MathF.Abs(sInt - sNorm);

            if (qDiff > rDiff && qDiff > sDiff)
                qInt = -rInt - sInt;
            else if (rDiff > sDiff)
                rInt = -qInt - sInt;

            
            int y = rInt;
            int x = qInt + ((rInt >= 0) ? (rInt / 2) : ((rInt - 1) / 2));

            return new VectorXYInt(x, y);
        }

        public static VectorXYInt ToXYIndexEvenR(this VectorXY point, float hexRadius, VectorXY hexFieldOrigin)
        {
            VectorQRS qrs;
            var shiftedPoint = point - hexFieldOrigin;

            var q = 0.5773502588f * shiftedPoint.X - 0.3333333333f * shiftedPoint.Y;
            var r = 0.6666666666f * shiftedPoint.Y;

            var res = new VectorQRS(q, r);

            qrs = res;

            if (hexRadius == 0)
                throw new ArgumentOutOfRangeException(nameof(hexRadius), hexRadius, $"Couldn't devide on zero {nameof(hexRadius)}");

            var axialPoint = qrs / hexRadius;

            VectorQRSInt index;
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

            var shift = rInt % 2 == 0 ? 0 : 1;

            index = new VectorQRSInt(qInt + shift, rInt);

            int x = index.Q + (index.R >= 0 ? index.R / 2 : (index.R - 1) / 2);
            int y = index.R;

            return new VectorXYInt(x, y);
        }

        public static VectorXYInt ToXYIndexOddQ(this VectorXY point, float hexRadius, VectorXY hexFieldOrigin)
        {
            VectorQRS qrs;
            var shiftedPoint = point - hexFieldOrigin;

            var q = 0.6666666666f * shiftedPoint.X;
            var r = 0.5773502588f * shiftedPoint.Y - 0.3333333333f * shiftedPoint.X;

            var res = new VectorQRS(q, r);

            qrs = res;

            if (hexRadius == 0)
                throw new ArgumentOutOfRangeException(nameof(hexRadius), hexRadius, $"Couldn't devide on zero {nameof(hexRadius)}");

            var axialPoint = qrs / hexRadius;

            VectorQRSInt index;
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

            index = new VectorQRSInt(qInt, rInt);

            int x = index.Q;
            int y = index.R + (index.Q >= 0 ? index.Q / 2 : (index.Q - 1) / 2);

            return new VectorXYInt(x, y);
        }

        public static VectorXYInt ToXYIndexEvenQ(this VectorXY point, float hexRadius, VectorXY hexFieldOrigin)
        {
            VectorQRS qrs;
            var shiftedPoint = point - hexFieldOrigin;

            var q = 0.6666666666f * shiftedPoint.X;
            var r = 0.5773502588f * shiftedPoint.Y - 0.3333333333f * shiftedPoint.X;

            var res = new VectorQRS(q, r);

            qrs = res;

            if (hexRadius == 0)
                throw new ArgumentOutOfRangeException(nameof(hexRadius), hexRadius, $"Couldn't devide on zero {nameof(hexRadius)}");

            var axialPoint = qrs / hexRadius;

            VectorQRSInt index;
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

            index = new VectorQRSInt(qInt, rInt + shift);

            int x = index.Q;
            int y = index.R + (index.Q >= 0 ? index.Q / 2 : (index.Q - 1) / 2);

            return new VectorXYInt(x, y);
        }

        
        public static VectorXYInt ToXYIndex(this VectorXY point, float hexRadius, VectorXY hexFieldOrigin, Layout layout)
        {
            VectorQRS qrs;
            if (layout.IsPointyTop())
            {
                var shiftedPoint = point - hexFieldOrigin;

                var q = 0.5773502588f * shiftedPoint.X - 0.3333333333f * shiftedPoint.Y;
                var r = 0.6666666666f * shiftedPoint.Y;

                var res = new VectorQRS(q, r);

                qrs = res;
            }
            else
            {
                var shiftedPoint = point - hexFieldOrigin;

                var q = 0.6666666666f * shiftedPoint.X;
                var r = 0.5773502588f * shiftedPoint.Y - 0.3333333333f * shiftedPoint.X;

                var res = new VectorQRS(q, r);

                qrs = res;
            }

            if (hexRadius == 0)
                throw new ArgumentOutOfRangeException(nameof(hexRadius), hexRadius, $"Couldn't devide on zero {nameof(hexRadius)}");

            var axialPoint = qrs / hexRadius;

            VectorQRSInt index;
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

                        index = new VectorQRSInt(qInt, rInt);
                        break;
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

                        var shift = rInt % 2 == 0 ? 0 : 1;

                        index = new VectorQRSInt(qInt + shift, rInt);
                        break;
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

                        index = new VectorQRSInt(qInt, rInt);
                        break;
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

                        index = new VectorQRSInt(qInt, rInt + shift);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }

            if (layout.IsPointyTop())
            {
                int x = index.Q + (index.R >= 0 ? index.R / 2 : (index.R - 1) / 2);
                int y = index.R;

                return new VectorXYInt(x, y);
            }
            else
            {
                int x = index.Q;
                int y = index.R + (index.Q >= 0 ? index.Q / 2 : (index.Q - 1) / 2);

                return new VectorXYInt(x, y);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXYInt ToXYIndex2(this VectorXY point, float hexRadius, VectorXY hexFieldOrigin, Layout layout)
        {
            return point
                .ToQRS(hexFieldOrigin, layout)
                .ToNormalizedAxial(hexRadius)
                .ToQRSIndex(layout)
                .ToXYIndex(layout);
        }
    }
}
