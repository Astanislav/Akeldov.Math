using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;
using BenchmarkDotNet.Attributes;

namespace Akeldov.Math.Spatial2D.Benchmarks.Sampling;

[MemoryDiagnoser]
[ShortRunJob]
public class PoissonDiskPointSamplerBenchmarks
{
    private VectorXY _fieldSize;
    private RadialDistanceField _distanceField = null!;

    [Params(100f, 200f)]
    public float FieldEdge { get; set; }

    [Params(4f, 8f)]
    public float MinimalDistance { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _fieldSize = new VectorXY(FieldEdge, FieldEdge);
        _distanceField = new RadialDistanceField(_fieldSize, MinimalDistance, MinimalDistance * 2f);
    }

    [Benchmark]
    public int SampleConstantDistance()
    {
        var sampler = new PoissonDiskPointSampler(new Random(45678), maxAttempts: 30);
        return sampler.Sample(_fieldSize, MinimalDistance).Count;
    }

    [Benchmark]
    public int SampleVariableDistance()
    {
        var sampler = new PoissonDiskPointSampler(new Random(45678), maxAttempts: 30);
        return sampler.Sample(_fieldSize, _distanceField).Count;
    }

    private sealed class RadialDistanceField : IFloatField
    {
        private readonly PointXY _center;
        private readonly float _maxDistanceToCorner;

        public RadialDistanceField(VectorXY fieldSize, float min, float max)
        {
            Min = min;
            Max = max;
            _center = new PointXY(fieldSize.X * 0.5f, fieldSize.Y * 0.5f);
            _maxDistanceToCorner = _center.Distance(new PointXY(0f, 0f));
        }

        public float Min { get; }

        public float Max { get; }

        public float Sample(PointXY point)
        {
            float t = point.Distance(_center) / _maxDistanceToCorner;
            return Min + (Max - Min) * t;
        }
    }
}
