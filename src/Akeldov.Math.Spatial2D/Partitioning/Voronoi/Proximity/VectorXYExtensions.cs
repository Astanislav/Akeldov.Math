using System.Runtime.CompilerServices;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    internal static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndexOfClosest(this Site[] sites, VectorXY point)
        {
            var px = point.X; var py = point.Y;
            float bestWeightedDistance = float.PositiveInfinity;
            int bestWeightedIndex = 0;
            float bestInfiniteDistance = float.PositiveInfinity;
            int bestInfiniteIndex = -1;

            int i = 0, n = sites.Length;
            for (; i + 3 < n; i += 4)
            {
                ref readonly var s0 = ref sites[i + 0];
                ref readonly var s1 = ref sites[i + 1];
                ref readonly var s2 = ref sites[i + 2];
                ref readonly var s3 = ref sites[i + 3];

                if (TryUpdate(
                    ref bestWeightedDistance,
                    ref bestWeightedIndex,
                    ref bestInfiniteDistance,
                    ref bestInfiniteIndex,
                    i + 0,
                    px,
                    py,
                    s0.Point.X,
                    s0.Point.Y,
                    s0.Power))
                    return i + 0;

                if (TryUpdate(
                    ref bestWeightedDistance,
                    ref bestWeightedIndex,
                    ref bestInfiniteDistance,
                    ref bestInfiniteIndex,
                    i + 1,
                    px,
                    py,
                    s1.Point.X,
                    s1.Point.Y,
                    s1.Power))
                    return i + 1;

                if (TryUpdate(
                    ref bestWeightedDistance,
                    ref bestWeightedIndex,
                    ref bestInfiniteDistance,
                    ref bestInfiniteIndex,
                    i + 2,
                    px,
                    py,
                    s2.Point.X,
                    s2.Point.Y,
                    s2.Power))
                    return i + 2;

                if (TryUpdate(
                    ref bestWeightedDistance,
                    ref bestWeightedIndex,
                    ref bestInfiniteDistance,
                    ref bestInfiniteIndex,
                    i + 3,
                    px,
                    py,
                    s3.Point.X,
                    s3.Point.Y,
                    s3.Power))
                    return i + 3;
            }
            for (; i < n; i++)
            {
                ref readonly var s = ref sites[i];
                if (TryUpdate(
                    ref bestWeightedDistance,
                    ref bestWeightedIndex,
                    ref bestInfiniteDistance,
                    ref bestInfiniteIndex,
                    i,
                    px,
                    py,
                    s.Point.X,
                    s.Point.Y,
                    s.Power))
                    return i;
            }

            return bestInfiniteIndex >= 0 ? bestInfiniteIndex : bestWeightedIndex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryUpdate(
            ref float bestWeightedDistance,
            ref int bestWeightedIndex,
            ref float bestInfiniteDistance,
            ref int bestInfiniteIndex,
            int idx,
            float px,
            float py,
            float x,
            float y,
            float power)
        {
            float dx = x - px; float dy = y - py;
            float d2 = dx * dx + dy * dy;

            if (d2 <= GeometryConstants.GeometryEpsilonSquared)
                return true;

            if (float.IsPositiveInfinity(power))
            {
                if (d2 < bestInfiniteDistance)
                {
                    bestInfiniteDistance = d2;
                    bestInfiniteIndex = idx;
                }

                return false;
            }

            if (power == 0f)
                return false;

            float weightedD2 = d2 / (power * power);
            if (weightedD2 < bestWeightedDistance)
            {
                bestWeightedDistance = weightedD2;
                bestWeightedIndex = idx;
            }

            return false;
        }
    }
}
