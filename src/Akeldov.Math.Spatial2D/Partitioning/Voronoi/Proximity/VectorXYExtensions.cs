using System.Runtime.CompilerServices;

namespace Akeldov.Math.Spatial2D.Partitioning.Voronoi
{
    internal static partial class VectorXYExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndexOfClosest(this Site[] sites, VectorXY point)
        {
            var px = point.X; var py = point.Y;
            float best = float.PositiveInfinity;
            int bestIdx = 0;

            int i = 0, n = sites.Length;
            for (; i + 3 < n; i += 4)
            {
                ref readonly var s0 = ref sites[i + 0];
                ref readonly var s1 = ref sites[i + 1];
                ref readonly var s2 = ref sites[i + 2];
                ref readonly var s3 = ref sites[i + 3];

                Update(ref best, ref bestIdx, i + 0, px, py, s0.Point.X, s0.Point.Y, s0.Power);
                Update(ref best, ref bestIdx, i + 1, px, py, s1.Point.X, s1.Point.Y, s1.Power);
                Update(ref best, ref bestIdx, i + 2, px, py, s2.Point.X, s2.Point.Y, s2.Power);
                Update(ref best, ref bestIdx, i + 3, px, py, s3.Point.X, s3.Point.Y, s3.Power);
            }
            for (; i < n; i++)
            {
                ref readonly var s = ref sites[i];
                Update(ref best, ref bestIdx, i, px, py, s.Point.X, s.Point.Y, s.Power);
            }
            return bestIdx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Update(ref float best, ref int bestIdx, int idx, float px, float py, float x, float y, float power)
        {
            float dx = x - px; float dy = y - py;
            float weightedD2 = (dx * dx + dy * dy) / (power * power);
            if (weightedD2 < best) { best = weightedD2; bestIdx = idx; }
        }
    }
}
