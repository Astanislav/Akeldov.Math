using System.Runtime.CompilerServices;

namespace Akeldov.Math.Hexes.Geometry
{
    public static class FloatExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ConvertHexApothemToRadius(this float apothem)
        {
            return Constants.Apothem2Radius * apothem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ConvertHexRadiusToApothem(this float radius)
        {
            return Constants.Radius2Apothem * radius;
        }
    }
}