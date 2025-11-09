namespace Akeldov.Math.Vectors.XY
{
    public static partial class VectorXYExtensions
    {
        public static VectorXY[] Round(this VectorXY[] vectors, int digits)
        {
            var res = new VectorXY[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                res[i] = vectors[i].Round(digits);
            }
            return res;
        }

        public static VectorXY Round(this VectorXY vector, int digits)
        {
            return new VectorXY(System.MathF.Round(vector.X, digits), System.MathF.Round(vector.Y, digits));
        }
    }
}
