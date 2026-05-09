namespace Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk
{
    public readonly struct PoissonDiskPointSample
    {
        public PoissonDiskPointSample(VectorXY point, float minimalDistance)
        {
            Point = point;
            MinimalDistance = minimalDistance;
        }

        public VectorXY Point { get; }

        public float MinimalDistance { get; }

        public void Deconstruct(out VectorXY point, out float minimalDistance)
        {
            point = Point;
            minimalDistance = MinimalDistance;
        }
    }
}
