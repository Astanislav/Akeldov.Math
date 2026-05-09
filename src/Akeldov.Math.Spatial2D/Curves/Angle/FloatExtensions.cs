using System;

namespace Akeldov.Math.Spatial2D.Curves
{
    public static class FloatExtensions
    {
        public static float NormalizeAngleRad(this float angle)
        {
            float twoPi = 2f * MathF.PI;
            angle %= twoPi;
            if (angle < 0f) angle += twoPi;
            return angle;
        }

        public static bool IsAngleWithinArc(this float angle, float startAngle, float endAngle)
        {
            if (startAngle.AlmostEquals(endAngle))
                return angle.AlmostEquals(startAngle);

            if (startAngle < endAngle)
                return angle >= startAngle && angle <= endAngle;
            else
                return angle >= startAngle || angle <= endAngle;
        }
    }
}
