using Akeldov.Math.Hexes.Vectors.QRS;

namespace Akeldov.Math.Hexes.Tests.VectorsQRS;

public class VectorQRSTests
{
    [Test]
    public void Constructor_SetsQRSComponents()
    {
        var vector = new VectorQRS(2.5f, -4f);

        Assert.Multiple(() =>
        {
            Assert.That(vector.Q, Is.EqualTo(2.5f));
            Assert.That(vector.R, Is.EqualTo(-4f));
            Assert.That(vector.S, Is.EqualTo(1.5f));
        });
    }

    [Test]
    public void StaticVectors_ReturnExpectedValues()
    {
        Assert.Multiple(() =>
        {
            Assert.That(VectorQRS.Zero, Is.EqualTo(new VectorQRS(0f, 0f)));
            Assert.That(VectorQRS.One, Is.EqualTo(new VectorQRS(1f, 1f)));
        });
    }

    [Test]
    public void EqualityMembers_CompareQAndR()
    {
        var left = new VectorQRS(2f, -3f);
        var same = new VectorQRS(2f, -3f);
        var different = new VectorQRS(2f, -4f);

        Assert.Multiple(() =>
        {
            Assert.That(left.Equals(same), Is.True);
            Assert.That(left.Equals((object)same), Is.True);
            Assert.That(left.Equals((object?)null), Is.False);
            Assert.That(left == same, Is.True);
            Assert.That(left != different, Is.True);
            Assert.That(left.GetHashCode(), Is.EqualTo(same.GetHashCode()));
        });
    }

    [Test]
    public void ToString_ReturnsQAndR()
    {
        Assert.That(new VectorQRS(2.5f, -4f).ToString(), Is.EqualTo("(2.5, -4)"));
    }

    [Test]
    public void Deconstruct_ReturnsQAndR()
    {
        var (q, r) = new VectorQRS(2.5f, -4f);

        Assert.Multiple(() =>
        {
            Assert.That(q, Is.EqualTo(2.5f));
            Assert.That(r, Is.EqualTo(-4f));
        });
    }

    [Test]
    public void Operators_ReturnExpectedVectors()
    {
        var left = new VectorQRS(5f, -7f);
        var right = new VectorQRS(-2f, 3f);

        Assert.Multiple(() =>
        {
            Assert.That(left + right, Is.EqualTo(new VectorQRS(3f, -4f)));
            Assert.That(left - right, Is.EqualTo(new VectorQRS(7f, -10f)));
            Assert.That(left * 2f, Is.EqualTo(new VectorQRS(10f, -14f)));
            Assert.That(2f * left, Is.EqualTo(new VectorQRS(10f, -14f)));
            Assert.That(left / 2f, Is.EqualTo(new VectorQRS(2.5f, -3.5f)));
        });
    }

    [Test]
    public void Casts_ConvertBetweenIntAndFloatVectors()
    {
        VectorQRS fromInt = new VectorQRSInt(2, -3);
        var toInt = (VectorQRSInt)new VectorQRS(2.9f, -3.9f);

        Assert.Multiple(() =>
        {
            Assert.That(fromInt, Is.EqualTo(new VectorQRS(2f, -3f)));
            Assert.That(toInt, Is.EqualTo(new VectorQRSInt(2, -3)));
        });
    }
}
