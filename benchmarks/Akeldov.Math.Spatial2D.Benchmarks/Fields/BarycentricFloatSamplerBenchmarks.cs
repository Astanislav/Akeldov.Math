using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Fields;
using BenchmarkDotNet.Attributes;

namespace Akeldov.Math.Spatial2D.Benchmarks.Fields;

[MemoryDiagnoser]
[ShortRunJob]
public class BarycentricFloatSamplerBenchmarks
{
    private FloatPointInfluenceSource[] _sources = null!;
    private VectorXY[] _queries = null!;
    private BarycentricFloatSampler<FloatPointInfluenceSource> _sampler = null!;

    [Params(16, 128, 512)]
    public int SourceCount { get; set; }

    [Params(100)]
    public int QueryCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(34567);

        _sources = new FloatPointInfluenceSource[SourceCount];
        for (int i = 0; i < _sources.Length; i++)
        {
            _sources[i] = new FloatPointInfluenceSource(
                power: 0.5f + random.NextSingle() * 2f,
                position: NextPoint(random, 1000f),
                value: random.NextSingle() * 100f);
        }

        _queries = new VectorXY[QueryCount];
        for (int i = 0; i < _queries.Length; i++)
            _queries[i] = NextPoint(random, 1000f);

        _sampler = new BarycentricFloatSampler<FloatPointInfluenceSource>();
    }

    [Benchmark]
    public float SampleQueries()
    {
        float sum = 0f;

        for (int i = 0; i < _queries.Length; i++)
            sum += _sampler.Sample(_sources, _queries[i]);

        return sum;
    }

    private static VectorXY NextPoint(Random random, float size)
    {
        return new VectorXY(random.NextSingle() * size, random.NextSingle() * size);
    }
}
