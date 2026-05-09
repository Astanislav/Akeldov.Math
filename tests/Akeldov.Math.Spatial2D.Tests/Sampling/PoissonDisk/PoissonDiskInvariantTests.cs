using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;

namespace Akeldov.Math.Spatial2D.Tests.Sampling.PoissonDisk;

public class PoissonDiskInvariantTests
{
    [TestCase(1)]
    [TestCase(12345)]
    [TestCase(98765)]
    public void Sample_WithConstantMinimalDistance_KeepsEveryPairFarEnough(int seed)
    {
        var sampler = new PoissonDiskPointSampler(new Random(seed), maxAttempts: 30);

        var result = sampler.Sample(new VectorXY(120f, 80f), minimalDistance: 9f);

        AssertEveryPairIsFarEnough(result);
    }

    [TestCase(1)]
    [TestCase(12345)]
    [TestCase(98765)]
    public void Sample_WithVariableMinimalDistance_KeepsEveryPairFarEnough(int seed)
    {
        var sampler = new PoissonDiskPointSampler(new Random(seed), maxAttempts: 30);
        var field = new HorizontalGradientFloatField(min: 5f, max: 13f, width: 120f);

        var result = sampler.Sample(new VectorXY(120f, 80f), field);

        AssertEveryPairIsFarEnough(result);
    }

    private static void AssertEveryPairIsFarEnough(IReadOnlyList<PoissonDiskPointSample> samples)
    {
        for (int i = 0; i < samples.Count; i++)
        {
            var aSample = samples[i];

            for (int j = i + 1; j < samples.Count; j++)
            {
                var bSample = samples[j];
                float requiredDistance = MathF.Max(aSample.MinimalDistance, bSample.MinimalDistance);
                float actualDistance = aSample.Point.Distance(bSample.Point);

                Assert.That(
                    actualDistance,
                    Is.GreaterThanOrEqualTo(requiredDistance - GeometryConstants.GeometryEpsilon),
                    $"Samples {i} {aSample.Point} and {j} {bSample.Point} are too close.");
            }
        }
    }

    private sealed class HorizontalGradientFloatField : IFloatField
    {
        private readonly float _min;
        private readonly float _max;
        private readonly float _width;

        public HorizontalGradientFloatField(float min, float max, float width)
        {
            _min = min;
            _max = max;
            _width = width;
            DistinctValues = new[] { min, max };
        }

        public float Min => _min;

        public float Max => _max;

        public IReadOnlyList<float> DistinctValues { get; }

        public float Sample(VectorXY point)
        {
            float t = point.X / _width;
            t = MathF.Max(0f, MathF.Min(1f, t));
            return _min + (_max - _min) * t;
        }
    }
}
