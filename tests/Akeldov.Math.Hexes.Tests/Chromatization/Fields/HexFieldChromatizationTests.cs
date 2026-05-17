using Akeldov.Math.Hexes.Chromatization;
using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Tests.Chromatization;

public class HexFieldChromatizationTests
{
    [Test]
    public void ToHexFieldChromatization_MatchesSingleHexChromaticClass_ForEveryLayout()
    {
        var resolution = new VectorXYInt(5, 4);

        foreach (Layout layout in Enum.GetValues(typeof(Layout)))
        {
            HexFieldChromatization chromatization = resolution.ToHexFieldChromatization(layout);

            Assert.That(chromatization.Width, Is.EqualTo(resolution.X));
            Assert.That(chromatization.Height, Is.EqualTo(resolution.Y));
            Assert.That(chromatization.ChromaticIndices, Has.Length.EqualTo(resolution.X * resolution.Y));

            for (int y = 0; y < resolution.Y; y++)
            {
                int rowStart = y * resolution.X;

                for (int x = 0; x < resolution.X; x++)
                {
                    byte expected = (byte)new VectorXYInt(x, y).GetChromaticClass(layout);

                    Assert.That(chromatization.ChromaticIndices[rowStart + x], Is.EqualTo(expected));
                }
            }
        }
    }

    [Test]
    public void ToHexFieldChromatization_WhenResolutionHasNegativeComponent_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new VectorXYInt(-1, 1).ToHexFieldChromatization(Layout.OddR));
    }

    [Test]
    public void ToHexFieldChromatization_WhenLayoutIsUnsupported_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new VectorXYInt(0, 0).ToHexFieldChromatization((Layout)42));
    }
}
