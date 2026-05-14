using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Tests.VectorsQRS;

public class TransformationTests
{
    [TestCase(SixfoldAngle.Deg0, 2f, -5f)]
    [TestCase(SixfoldAngle.Deg60, 5f, -3f)]
    [TestCase(SixfoldAngle.Deg120, 3f, 2f)]
    [TestCase(SixfoldAngle.Deg180, -2f, 5f)]
    [TestCase(SixfoldAngle.Deg240, -5f, 3f)]
    [TestCase(SixfoldAngle.Deg300, -3f, -2f)]
    public void VectorQRS_RotateBySixfoldAngle_ReturnsExpectedVector(
        SixfoldAngle angle,
        float expectedQ,
        float expectedR)
    {
        var actual = new VectorQRS(2f, -5f).Rotate(angle);

        VectorAssert.AreEqual(actual, expectedQ, expectedR);
    }

    [Test]
    public void VectorQRS_RotateByRadians_ReturnsExpectedVector()
    {
        var actual = new VectorQRS(2f, 3f).Rotate(MathF.PI / 2f);

        VectorAssert.AreEqual(actual, -3f, 2f);
    }

    [TestCase(SixfoldAngle.Deg0, 2, -5)]
    [TestCase(SixfoldAngle.Deg60, 5, -3)]
    [TestCase(SixfoldAngle.Deg120, 3, 2)]
    [TestCase(SixfoldAngle.Deg180, -2, 5)]
    [TestCase(SixfoldAngle.Deg240, -5, 3)]
    [TestCase(SixfoldAngle.Deg300, -3, -2)]
    public void VectorQRSInt_RotateBySixfoldAngle_ReturnsExpectedVector(
        SixfoldAngle angle,
        int expectedQ,
        int expectedR)
    {
        var actual = new VectorQRSInt(2, -5).Rotate(angle);

        Assert.That(actual, Is.EqualTo(new VectorQRSInt(expectedQ, expectedR)));
    }

    [Test]
    public void VectorQRSInt_RotateByRadians_ReturnsExpectedVector()
    {
        var actual = new VectorQRSInt(2, 3).Rotate(MathF.PI / 2f);

        VectorAssert.AreEqual(actual, -3f, 2f);
    }

    [Test]
    public void QRSRotations_ThrowForInvalidSixfoldAngle()
    {
        var invalid = (SixfoldAngle)42;

        Assert.Multiple(() =>
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new VectorQRS(1f, 2f).Rotate(invalid));
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = new VectorQRSInt(1, 2).Rotate(invalid));
        });
    }

    [Test]
    public void VectorXY_RotateBySixfoldAngle_ReturnsExpectedVector()
    {
        var actual = new VectorXY(2f, 0f).Rotate(SixfoldAngle.Deg60);

        VectorAssert.AreEqual(actual, 1f, 1.7320508f);
    }

    [Test]
    public void VectorXYInt_RotateBySixfoldAngle_ReturnsExpectedVector()
    {
        var actual = new VectorXYInt(2, -3).Rotate(SixfoldAngle.Deg180);

        VectorAssert.AreEqual(actual, -2f, 3f);
    }

    [Test]
    public void VectorXY_TransformOverloads_ReturnExpectedVectors()
    {
        var point = new VectorXY(2f, 0f);
        var offset = new VectorXY(10f, 20f);
        var intOffset = new VectorXYInt(10, 20);

        Assert.Multiple(() =>
        {
            VectorAssert.AreEqual(point.Transform(SixfoldAngle.Deg60, offset), 11f, 21.73205f);
            VectorAssert.AreEqual(point.Transform(SixfoldAngle.Deg60, intOffset), 11f, 21.73205f);
            VectorAssert.AreEqual(point.Transform(2f, SixfoldAngle.Deg60, offset), 12f, 23.464102f);
            VectorAssert.AreEqual(point.Transform(2f, SixfoldAngle.Deg60, intOffset), 12f, 23.464102f);
        });
    }

    [Test]
    public void VectorXYInt_TransformOverloads_ReturnExpectedVectors()
    {
        var point = new VectorXYInt(2, 0);
        var offset = new VectorXY(10f, 20f);
        var intOffset = new VectorXYInt(10, 20);

        Assert.Multiple(() =>
        {
            VectorAssert.AreEqual(point.Transform(SixfoldAngle.Deg60, offset), 11f, 21.73205f);
            VectorAssert.AreEqual(point.Transform(SixfoldAngle.Deg60, intOffset), 11f, 21.73205f);
            VectorAssert.AreEqual(point.Transform(2f, SixfoldAngle.Deg60, offset), 12f, 23.464102f);
            VectorAssert.AreEqual(point.Transform(2f, SixfoldAngle.Deg60, intOffset), 12f, 23.464102f);
        });
    }

    [Test]
    public void VectorXY_RotateAroundPivotOverloads_ReturnExpectedVectors()
    {
        var point = new VectorXY(3f, 1f);
        var pivot = new VectorXY(1f, 1f);
        var intPivot = new VectorXYInt(1, 1);

        Assert.Multiple(() =>
        {
            VectorAssert.AreEqual(point.Rotate(pivot, SixfoldAngle.Deg60), 2f, 2.7320508f);
            VectorAssert.AreEqual(point.Rotate(intPivot, SixfoldAngle.Deg60), 2f, 2.7320508f);
        });
    }

    [Test]
    public void VectorXYInt_RotateAroundPivotOverloads_ReturnExpectedVectors()
    {
        var point = new VectorXYInt(3, 1);
        var pivot = new VectorXY(1f, 1f);
        var intPivot = new VectorXYInt(1, 1);

        Assert.Multiple(() =>
        {
            VectorAssert.AreEqual(point.Rotate(pivot, SixfoldAngle.Deg60), 2f, 2.7320508f);
            VectorAssert.AreEqual(point.Rotate(intPivot, SixfoldAngle.Deg60), 2f, 2.7320508f);
        });
    }
}
