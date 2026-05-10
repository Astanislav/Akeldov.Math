using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D
{
    public static class PositionedCollectionExtensions
    {
        public static VectorXY GetBarycenter<Texel>(this IReadOnlyList<Texel> texels)
            where Texel : IHasPosition2D
        {
            var sum = VectorXY.Zero;
            for (int k = 0; k < texels.Count; k++)
            {
                sum = sum + texels[k].Center;
            }
            var res = sum / texels.Count;
            return res;
        }

        public static VectorXY GetBarycenter<Texel>(this Texel[] texels)
            where Texel : IHasPosition2D
        {
            var sum = VectorXY.Zero;
            for (int k = 0; k < texels.Length; k++)
            {
                sum = sum + texels[k].Center;
            }
            var res = sum / texels.Length;
            return res;
        }

        public static Texel GetBarycentric<Texel>(this IReadOnlyList<Texel> texels)
            where Texel : IHasPosition2D
        {
            var barycenter = texels.GetBarycenter();

            var minDist = float.MaxValue;
            var baricentricTexel = texels[0];
            for (int k = 1; k < texels.Count; k++)
            {
                var texel = texels[k];
                var distance = texel.Center.Distance(barycenter);

                if (distance < minDist)
                {
                    minDist = distance;
                    baricentricTexel = texel;
                }
            }
            return baricentricTexel;
        }

        public static Texel GetBarycentric<Texel>(this Texel[] texels)
            where Texel : IHasPosition2D
        {
            var barycenter = texels.GetBarycenter();

            var minDist = float.MaxValue;
            var baricentricTexel = texels[0];
            for (int k = 1; k < texels.Length; k++)
            {
                var texel = texels[k];
                var distance = texel.Center.Distance(barycenter);

                if (distance < minDist)
                {
                    minDist = distance;
                    baricentricTexel = texel;
                }
            }
            return baricentricTexel;
        }

        public static Texel GetClosestTo<Texel>(this Texel[] texels, VectorXY point)
            where Texel : IHasPosition2D
        {
            var minDist = float.MaxValue;
            var baricentricTexel = texels[0];
            for (int k = 1; k < texels.Length; k++)
            {
                var texel = texels[k];
                var distance = texel.Center.Distance(point);

                if (distance < minDist)
                {
                    minDist = distance;
                    baricentricTexel = texel;
                }
            }
            return baricentricTexel;
        }

        public static Texel GetClosestTo<Texel>(this IReadOnlyList<Texel> texels, VectorXY point)
            where Texel : IHasPosition2D
        {
            var minDist = float.MaxValue;
            var baricentricTexel = texels[0];
            for (int k = 1; k < texels.Count; k++)
            {
                var texel = texels[k];
                var distance = texel.Center.Distance(point);

                if (distance < minDist)
                {
                    minDist = distance;
                    baricentricTexel = texel;
                }
            }
            return baricentricTexel;
        }
    }
}
