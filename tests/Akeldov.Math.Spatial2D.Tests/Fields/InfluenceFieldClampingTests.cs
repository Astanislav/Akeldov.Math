using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Fields;

namespace Akeldov.Math.Spatial2D.Tests.Fields;

public class InfluenceFieldClampingTests
{
    [Test]
    public void PointInfluenceFloatField_WhenSourceListChangesAfterConstruction_UsesOriginalSources()
    {
        var sources = new List<FloatPointInfluenceSource>
        {
            new FloatPointInfluenceSource(1f, VectorXY.Zero, 0f),
            new FloatPointInfluenceSource(1f, new VectorXY(10f, 0f), 10f)
        };
        var field = new PointInfluenceFloatField(
            new NearestFloatInfluenceSampler<FloatPointInfluenceSource>(),
            sources);

        sources.Clear();
        sources.Add(new FloatPointInfluenceSource(1f, VectorXY.Zero, 100f));

        Assert.That(field.InfluencePoints, Has.Count.EqualTo(2));
        Assert.That(field.Min, Is.EqualTo(0f));
        Assert.That(field.Max, Is.EqualTo(10f));
        Assert.That(field.Sample(new VectorXY(10f, 0f)), Is.EqualTo(10f));
    }

    [Test]
    public void PointInfluenceFloatField_WhenSamplerReturnsBelowMin_ClampsToMin()
    {
        var field = new PointInfluenceFloatField(
            new ConstantSampler<FloatPointInfluenceSource, float>(-10f),
            new[]
            {
                new FloatPointInfluenceSource(1f, VectorXY.Zero, 0f),
                new FloatPointInfluenceSource(1f, VectorXY.One, 5f)
            });

        float value = field.Sample(new VectorXY(10f, 10f));

        Assert.That(value, Is.EqualTo(0f));
    }

    [Test]
    public void PointInfluenceFloatField_WhenSamplerReturnsNaN_Throws()
    {
        var field = new PointInfluenceFloatField(
            new ConstantSampler<FloatPointInfluenceSource, float>(float.NaN),
            new[]
            {
                new FloatPointInfluenceSource(1f, VectorXY.Zero, 0f),
                new FloatPointInfluenceSource(1f, VectorXY.One, 5f)
            });

        Assert.Throws<InvalidOperationException>(() => field.Sample(new VectorXY(10f, 10f)));
    }

    [Test]
    public void PointInfluenceIntField_WhenSamplerReturnsAboveMax_ClampsToMax()
    {
        var field = new PointInfluenceIntField(
            new ConstantSampler<IntPointInfluenceSource, int>(30),
            new[]
            {
                new IntPointInfluenceSource(1f, VectorXY.Zero, 2),
                new IntPointInfluenceSource(1f, VectorXY.One, 7)
            });

        int value = field.Sample(new VectorXY(10f, 10f));

        Assert.That(value, Is.EqualTo(7));
    }

    [Test]
    public void CurveInfluenceFloatField_WhenSamplerReturnsOutsideRange_ClampsToRange()
    {
        var field = new CurveInfluenceFloatField(
            new ConstantSampler<ICurveInfluenceSource<float>, float>(100f),
            CreateCurveSources(),
            min: -2f,
            max: 3f);

        float value = field.Sample(new VectorXY(10f, 10f));

        Assert.That(value, Is.EqualTo(3f));
    }

    [Test]
    public void CurveInfluenceFloatField_WhenSamplerReturnsNaN_Throws()
    {
        var field = new CurveInfluenceFloatField(
            new ConstantSampler<ICurveInfluenceSource<float>, float>(float.NaN),
            CreateCurveSources(),
            min: -2f,
            max: 3f);

        Assert.Throws<InvalidOperationException>(() => field.Sample(new VectorXY(10f, 10f)));
    }

    [TestCase(3f, 2f, "min")]
    [TestCase(float.NaN, 2f, "min")]
    [TestCase(1f, float.NaN, "max")]
    public void CurveInfluenceFloatField_WhenRangeIsInvalid_Throws(float min, float max, string paramName)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CurveInfluenceFloatField(
                new ConstantSampler<ICurveInfluenceSource<float>, float>(0f),
                CreateCurveSources(),
                min,
                max));

        Assert.That(exception!.ParamName, Is.EqualTo(paramName));
    }

    [TestCase(3f, 2f, "min")]
    [TestCase(float.NaN, 2f, "min")]
    [TestCase(1f, float.NaN, "max")]
    public void CurveInfluenceFloatField_WithCuller_WhenRangeIsInvalid_Throws(float min, float max, string paramName)
    {
        var sources = CreateCurveSources();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CurveInfluenceFloatField(
                new ConstantSampler<ICurveInfluenceSource<float>, float>(0f),
                sources,
                min,
                max,
                new FixedCurveCuller(sources)));

        Assert.That(exception!.ParamName, Is.EqualTo(paramName));
    }

    private static ICurveInfluenceSource<float>[] CreateCurveSources()
    {
        var curve = new Segment(VectorXY.Zero, new VectorXY(10f, 0f));

        return new ICurveInfluenceSource<float>[]
        {
            new FloatCurveInfluenceSource(1f, curve, 0f)
        };
    }

    private sealed class ConstantSampler<TSource, TValue> : IInfluenceSampler<TSource, TValue>
        where TSource : IInfluenceSource<TValue>
    {
        private readonly TValue _value;

        public ConstantSampler(TValue value)
        {
            _value = value;
        }

        public TValue Sample(IReadOnlyList<TSource> influenceSources, VectorXY point)
        {
            return _value;
        }
    }

    private sealed class FixedCurveCuller : IInfluenceSourceCuller<ICurveInfluenceSource<float>>
    {
        private readonly List<ICurveInfluenceSource<float>> _sources;

        public FixedCurveCuller(IReadOnlyList<ICurveInfluenceSource<float>> sources)
        {
            _sources = new List<ICurveInfluenceSource<float>>(sources);
        }

        public List<ICurveInfluenceSource<float>> Cull(VectorXY point)
        {
            return _sources;
        }
    }
}
