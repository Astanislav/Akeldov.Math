namespace Akeldov.Math.Spatial2D.Tests.Scaling;

public class IReadOnlyListExtensionsTests
{
    [Test]
    public void Scale_WhenItemsAreNull_Throws()
    {
        IReadOnlyList<TestScalable> items = null!;

        var exception = Assert.Throws<ArgumentNullException>(() =>
            items.Scale(new VectorXY(2f, 3f)));

        Assert.That(exception!.ParamName, Is.EqualTo("items"));
    }

    [Test]
    public void Scale_WhenItemsAreEmpty_Throws()
    {
        var items = Array.Empty<TestScalable>();

        var exception = Assert.Throws<ArgumentException>(() =>
            items.Scale(new VectorXY(2f, 3f)));

        Assert.That(exception!.ParamName, Is.EqualTo("items"));
    }

    [Test]
    public void Scale_WhenItemsContainNull_Throws()
    {
        IReadOnlyList<TestScalable> items = new TestScalable[] { null! };

        var exception = Assert.Throws<ArgumentException>(() =>
            items.Scale(new VectorXY(2f, 3f)));

        Assert.That(exception!.ParamName, Is.EqualTo("items"));
    }

    [Test]
    public void Scale_WhenItemsAreValid_ReturnsScaledItems()
    {
        IReadOnlyList<TestScalable> items = new[]
        {
            new TestScalable(new VectorXY(2f, 3f)),
            new TestScalable(new VectorXY(-1f, 4f))
        };

        var scaled = items.Scale(new VectorXY(3f, 2f));

        Assert.That(scaled, Has.Count.EqualTo(2));
        AssertVector(scaled[0].Value, 6f, 6f);
        AssertVector(scaled[1].Value, -3f, 8f);
    }

    private static void AssertVector(VectorXY actual, float expectedX, float expectedY)
    {
        Assert.That(actual.X, Is.EqualTo(expectedX).Within(GeometryConstants.GeometryEpsilon));
        Assert.That(actual.Y, Is.EqualTo(expectedY).Within(GeometryConstants.GeometryEpsilon));
    }

    private sealed class TestScalable : IScalable<TestScalable>
    {
        public TestScalable(VectorXY value)
        {
            Value = value;
        }

        public VectorXY Value { get; }

        public TestScalable Scale(VectorXY scale)
        {
            return new TestScalable(new VectorXY(Value.X * scale.X, Value.Y * scale.Y));
        }
    }
}
