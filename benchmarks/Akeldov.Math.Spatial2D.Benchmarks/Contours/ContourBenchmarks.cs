using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;
using BenchmarkDotNet.Attributes;

namespace Akeldov.Math.Spatial2D.Benchmarks.Contours;

[MemoryDiagnoser]
[ShortRunJob]
public class ContourBenchmarks
{
    private Contour _contour = null!;
    private VectorXY[] _queries = null!;

    [Params(8, 64)]
    public int SegmentCount { get; set; }

    [Params(1_000)]
    public int QueryCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _contour = CreateRegularPolygonContour(SegmentCount, 100f);
        _queries = CreateQueries(QueryCount, 120f);
    }

    [Benchmark]
    public int EnclosesQueries()
    {
        int enclosedCount = 0;

        for (int i = 0; i < _queries.Length; i++)
        {
            if (_contour.Encloses(_queries[i]))
                enclosedCount++;
        }

        return enclosedCount;
    }

    [Benchmark]
    public Contour FilletCorners()
    {
        return _contour.FilletCorners(2f);
    }

    private static Contour CreateRegularPolygonContour(int segmentCount, float radius)
    {
        var curves = new IFinitePath[segmentCount];

        for (int i = 0; i < curves.Length; i++)
        {
            VectorXY start = GetRegularPolygonVertex(i, segmentCount, radius);
            VectorXY end = GetRegularPolygonVertex((i + 1) % segmentCount, segmentCount, radius);
            curves[i] = new ParameterizedSegment(start, end);
        }

        return new Contour(curves);
    }

    private static VectorXY GetRegularPolygonVertex(int index, int segmentCount, float radius)
    {
        float angle = index * 2f * MathF.PI / segmentCount;
        return new VectorXY(MathF.Cos(angle) * radius, MathF.Sin(angle) * radius);
    }

    private static VectorXY[] CreateQueries(int queryCount, float range)
    {
        var random = new Random(45678);
        var queries = new VectorXY[queryCount];
        float offset = range * 0.5f;

        for (int i = 0; i < queries.Length; i++)
        {
            queries[i] = new VectorXY(
                random.NextSingle() * range - offset,
                random.NextSingle() * range - offset);
        }

        return queries;
    }
}
