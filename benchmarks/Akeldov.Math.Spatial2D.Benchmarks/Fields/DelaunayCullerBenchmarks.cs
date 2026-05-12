using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Fields;
using BenchmarkDotNet.Attributes;

namespace Akeldov.Math.Spatial2D.Benchmarks.Fields;

[MemoryDiagnoser]
[ShortRunJob]
public class DelaunayCullerBenchmarks
{
    private FloatPointInfluenceSource[] _sources = null!;
    private VectorXY[] _queries = null!;
    private DelaunayCuller<FloatPointInfluenceSource> _culler = null!;

    [Params(32, 128, 512)]
    public int SourceCount { get; set; }

    [Params(1_000)]
    public int QueryCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(23456);

        _sources = new FloatPointInfluenceSource[SourceCount];
        for (int i = 0; i < _sources.Length; i++)
        {
            _sources[i] = new FloatPointInfluenceSource(
                power: 1f,
                position: NextPoint(random, 1000f),
                value: random.NextSingle());
        }

        _queries = new VectorXY[QueryCount];
        for (int i = 0; i < _queries.Length; i++)
            _queries[i] = NextPoint(random, 1000f);

        _culler = new DelaunayCuller<FloatPointInfluenceSource>(_sources);
    }

    [Benchmark]
    public DelaunayCuller<FloatPointInfluenceSource> Build()
    {
        return new DelaunayCuller<FloatPointInfluenceSource>(_sources);
    }

    [Benchmark]
    public int CullQueries()
    {
        int selectedSourceCount = 0;

        for (int i = 0; i < _queries.Length; i++)
            selectedSourceCount += _culler.Cull(_queries[i]).Count;

        return selectedSourceCount;
    }

    private static VectorXY NextPoint(Random random, float size)
    {
        return new VectorXY(random.NextSingle() * size, random.NextSingle() * size);
    }
}
