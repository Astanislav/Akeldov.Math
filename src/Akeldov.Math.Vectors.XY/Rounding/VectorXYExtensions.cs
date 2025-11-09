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

        public static VectorXY[] Floor(this VectorXY[] vectors, int digits)
        {
            var res = new VectorXY[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                res[i] = vectors[i].Floor(digits);
            }
            return res;
        }

        public static VectorXY[] Ceiling(this VectorXY[] vectors, int digits)
        {
            var res = new VectorXY[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                res[i] = vectors[i].Ceiling(digits);
            }
            return res;
        }

        public static VectorXY Round(this VectorXY vector, int digits)
        {
            return new VectorXY(System.MathF.Round(vector.X, digits), System.MathF.Round(vector.Y, digits));
        }

        public static VectorXY Floor(this VectorXY vector, int digits)
        {
            return new VectorXY(System.MathF.Floor(vector.X), System.MathF.Floor(vector.Y));
        }

        public static VectorXY Ceiling(this VectorXY vector, int digits)
        {
            return new VectorXY(System.MathF.Ceiling(vector.X), System.MathF.Ceiling(vector.Y));
        }
    }
}
