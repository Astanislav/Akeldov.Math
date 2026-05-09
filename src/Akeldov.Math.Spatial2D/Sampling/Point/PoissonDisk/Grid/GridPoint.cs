namespace Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk
{
    internal readonly struct GridPoint
    {
        public GridPoint(VectorXY point, float minimalDistance)
        {
            Point = point;
            MinimalDistance = minimalDistance;
        }

        public VectorXY Point { get; }

        public float MinimalDistance { get; }
    }
}
