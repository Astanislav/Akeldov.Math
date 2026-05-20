using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Regions;
using BenchmarkDotNet.Attributes;

namespace Akeldov.Math.Spatial2D.Benchmarks.Regions;

[MemoryDiagnoser]
[ShortRunJob]
public class RegionBenchmarks
{
    private IContour[] _contours = null!;
    private Region _region = null!;
    private VectorXY[] _queries = null!;

    [Params(1, 4)]
    public int ContourCount { get; set; }

    [Params(1_000)]
    public int QueryCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _contours = CreateNestedSquareContours(ContourCount);
        _region = new Region(_contours);
        _queries = CreateQueries(QueryCount, 120f, -10f);
    }

    [Benchmark]
    public Region Construct()
    {
        return new Region(_contours);
    }

    [Benchmark]
    public int ContainsQueries()
    {
        int containedCount = 0;

        for (int i = 0; i < _queries.Length; i++)
        {
            if (_region.Contains(_queries[i]))
                containedCount++;
        }

        return containedCount;
    }

    private static IContour[] CreateNestedSquareContours(int contourCount)
    {
        var contours = new IContour[contourCount];

        for (int i = 0; i < contours.Length; i++)
        {
            float margin = i * 12f;
            contours[i] = CreateSquareContour(margin, margin, 100f - margin, 100f - margin);
        }

        return contours;
    }

    private static Contour CreateSquareContour(float left, float bottom, float right, float top)
    {
        return new Contour(new IFinitePath[]
        {
            new ParameterizedSegment(new VectorXY(left, bottom), new VectorXY(right, bottom)),
            new ParameterizedSegment(new VectorXY(right, bottom), new VectorXY(right, top)),
            new ParameterizedSegment(new VectorXY(right, top), new VectorXY(left, top)),
            new ParameterizedSegment(new VectorXY(left, top), new VectorXY(left, bottom))
        });
    }

    private static VectorXY[] CreateQueries(int queryCount, float range, float offset)
    {
        var random = new Random(56789);
        var queries = new VectorXY[queryCount];

        for (int i = 0; i < queries.Length; i++)
        {
            queries[i] = new VectorXY(
                offset + random.NextSingle() * range,
                offset + random.NextSingle() * range);
        }

        return queries;
    }
}
