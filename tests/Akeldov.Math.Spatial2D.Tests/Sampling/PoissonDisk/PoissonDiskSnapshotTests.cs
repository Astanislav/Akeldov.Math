using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;

namespace Akeldov.Math.Spatial2D.Tests.Sampling.PoissonDisk;

public class PoissonDiskSnapshotTests
{
    [Test]
    public void Sample_WithConstantMinimalDistance_MatchesApprovedImage()
    {
        var sampler = new PoissonDiskPointSampler(new Random(12345), maxAttempts: 30);

        var result = sampler.Sample(new VectorXY(120f, 80f), minimalDistance: 9f);

        Assert.That(result, Has.Count.EqualTo(77));
        AssertMatchesApprovedSvg("constant-distance.svg", result, new VectorXY(120f, 80f));
    }

    [Test]
    public void Sample_WithVariableMinimalDistance_MatchesApprovedImage()
    {
        var sampler = new PoissonDiskPointSampler(new Random(12345), maxAttempts: 30);
        var field = new HorizontalGradientFloatField(min: 5f, max: 13f, width: 120f);

        var result = sampler.Sample(new VectorXY(120f, 80f), field);

        Assert.That(result, Has.Count.EqualTo(104));
        AssertMatchesApprovedSvg("variable-distance.svg", result, new VectorXY(120f, 80f));
    }

    private static void AssertMatchesApprovedSvg(
        string approvedFileName,
        IReadOnlyList<PoissonDiskPointSample> result,
        VectorXY fieldSize)
    {
        string actual = PoissonDiskSvgRenderer.Render(result, fieldSize);
        string approvedPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Sampling", "PoissonDisk", "Approved", approvedFileName);

        if (!File.Exists(approvedPath))
        {
            string actualPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, approvedFileName.Replace(".svg", ".actual.svg"));
            File.WriteAllText(actualPath, actual);
            TestContext.AddTestAttachment(actualPath, "Actual Poisson disk image");
            Assert.Fail($"Poisson disk approved image is missing. Actual image: {actualPath}");
        }

        string approved = File.ReadAllText(approvedPath);

        if (actual != approved)
        {
            string actualPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, approvedFileName.Replace(".svg", ".actual.svg"));
            File.WriteAllText(actualPath, actual);
            TestContext.AddTestAttachment(actualPath, "Actual Poisson disk image");
            Assert.Fail($"Poisson disk snapshot changed. Actual image: {actualPath}");
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

        public float Sample(PointXY point)
        {
            float t = point.X / _width;
            t = MathF.Max(0f, MathF.Min(1f, t));
            return _min + (_max - _min) * t;
        }
    }
}
