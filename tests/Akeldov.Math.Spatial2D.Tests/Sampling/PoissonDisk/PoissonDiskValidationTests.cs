using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;

namespace Akeldov.Math.Spatial2D.Tests.Sampling.PoissonDisk;

public class PoissonDiskValidationTests
{
    [Test]
    public void Constructor_WhenRandomIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new PoissonDiskPointSampler(null!, maxAttempts: 30));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Constructor_WhenMaxAttemptsIsNotPositive_Throws(int maxAttempts)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PoissonDiskPointSampler(new Random(1), maxAttempts));
    }

    [TestCase(0f)]
    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void PointSampleConstructor_WhenMinimalDistanceIsInvalid_Throws(float minimalDistance)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new PoissonDiskPointSample(VectorXY.Zero, minimalDistance));

        Assert.That(exception!.ParamName, Is.EqualTo("minimalDistance"));
    }

    [Test]
    public void PointSampleConstructor_WhenMinimalDistanceIsValid_StoresSample()
    {
        var point = new VectorXY(1f, 2f);
        var sample = new PoissonDiskPointSample(point, 3f);

        Assert.That(sample.Point, Is.EqualTo(point));
        Assert.That(sample.MinimalDistance, Is.EqualTo(3f));
    }

    [TestCase(0f)]
    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Sample_WhenMinimalDistanceIsInvalid_Throws(float minimalDistance)
    {
        var sampler = new PoissonDiskPointSampler(new Random(1), maxAttempts: 30);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => sampler.Sample(new VectorXY(10f, 10f), minimalDistance));

        Assert.That(exception!.ParamName, Is.EqualTo("minimalDistance"));
    }

    [TestCase(0f, 10f)]
    [TestCase(10f, 0f)]
    [TestCase(-1f, 10f)]
    [TestCase(10f, -1f)]
    [TestCase(float.NaN, 10f)]
    [TestCase(10f, float.NaN)]
    [TestCase(float.PositiveInfinity, 10f)]
    [TestCase(10f, float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity, 10f)]
    [TestCase(10f, float.NegativeInfinity)]
    public void Sample_WhenFieldSizeIsInvalid_Throws(float width, float height)
    {
        var sampler = new PoissonDiskPointSampler(new Random(1), maxAttempts: 30);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => sampler.Sample(new VectorXY(width, height), 2f));

        Assert.That(exception!.ParamName, Is.EqualTo("fieldSize"));
    }

    [Test]
    public void Sample_WhenMinimalDistanceFieldIsNull_Throws()
    {
        var sampler = new PoissonDiskPointSampler(new Random(1), maxAttempts: 30);

        Assert.Throws<ArgumentNullException>(() => sampler.Sample(new VectorXY(10f, 10f), (IFloatField)null!));
    }

    [TestCase(0f, 1f)]
    [TestCase(1f, 0f)]
    [TestCase(-1f, 1f)]
    [TestCase(1f, -1f)]
    [TestCase(3f, 2f)]
    [TestCase(float.NaN, 1f)]
    [TestCase(1f, float.NaN)]
    [TestCase(float.PositiveInfinity, 1f)]
    [TestCase(1f, float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity, 1f)]
    [TestCase(1f, float.NegativeInfinity)]
    public void Sample_WhenMinimalDistanceFieldRangeIsInvalid_Throws(float min, float max)
    {
        var sampler = new PoissonDiskPointSampler(new Random(1), maxAttempts: 30);
        var field = new TestFloatField(min, max);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => sampler.Sample(new VectorXY(10f, 10f), field));

        Assert.That(exception!.ParamName, Is.EqualTo("minimalDistanceField"));
    }

    [TestCase(0f)]
    [TestCase(-1f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Sample_WhenMinimalDistanceFieldReturnsInvalidValue_Throws(float value)
    {
        var sampler = new PoissonDiskPointSampler(new Random(1), maxAttempts: 30);
        var field = new InvalidSampleFloatField(value);

        var exception = Assert.Throws<InvalidOperationException>(
            () => sampler.Sample(new VectorXY(10f, 10f), field));

        Assert.That(exception!.Message, Does.Contain("finite and positive"));
    }

    [Test]
    public void Sample_WhenParametersAreValid_ReturnsDistancesForEverySample()
    {
        var sampler = new PoissonDiskPointSampler(new Random(1), maxAttempts: 30);

        var result = sampler.Sample(new VectorXY(20f, 20f), 5f);

        Assert.That(result, Is.Not.Empty);
        Assert.That(result.All(sample => sample.MinimalDistance > 0f), Is.True);
    }

    [Test]
    public void Sample_WhenResultAccessed_ReturnsMutableList()
    {
        var sampler = new PoissonDiskPointSampler(new Random(1), maxAttempts: 30);

        List<PoissonDiskPointSample> result = sampler.Sample(new VectorXY(20f, 20f), 5f);
        int originalCount = result.Count;

        result.Add(new PoissonDiskPointSample(VectorXY.Zero, 1f));

        Assert.That(result, Has.Count.EqualTo(originalCount + 1));
    }

    [Test]
    public void Sample_WhenCandidateIsOutsideSamplingArea_DoesNotSampleMinimalDistanceFieldThere()
    {
        var random = new SequenceRandom(0.5, 0.5, 0.0, 0.5);
        var sampler = new PoissonDiskPointSampler(random, maxAttempts: 1);
        var field = new BoundedFloatField(new VectorXY(10f, 10f), 8f);

        Assert.DoesNotThrow(() => sampler.Sample(new VectorXY(10f, 10f), field));
    }

    private sealed class TestFloatField : IFloatField
    {
        public TestFloatField(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; }

        public float Max { get; }

        public float Sample(VectorXY point)
        {
            return Min;
        }
    }

    private sealed class BoundedFloatField : IFloatField
    {
        private readonly VectorXY _fieldSize;
        private readonly float _value;

        public BoundedFloatField(VectorXY fieldSize, float value)
        {
            _fieldSize = fieldSize;
            _value = value;
        }

        public float Min => _value;

        public float Max => _value;

        public float Sample(VectorXY point)
        {
            if (point.X < 0f || point.X >= _fieldSize.X || point.Y < 0f || point.Y >= _fieldSize.Y)
                throw new ArgumentOutOfRangeException(nameof(point));

            return _value;
        }
    }

    private sealed class InvalidSampleFloatField : IFloatField
    {
        private readonly float _value;

        public InvalidSampleFloatField(float value)
        {
            _value = value;
        }

        public float Min => 1f;

        public float Max => 1f;

        public float Sample(VectorXY point)
        {
            return _value;
        }
    }

    private sealed class SequenceRandom : Random
    {
        private readonly double[] _values;
        private int _index;

        public SequenceRandom(params double[] values)
        {
            _values = values;
        }

        public override int Next(int maxValue)
        {
            return 0;
        }

        public override double NextDouble()
        {
            return _values[_index++];
        }
    }
}
