using Akeldov.Math.Hexes.Chromatization;
using Akeldov.Math.Hexes.Geometry;
using Akeldov.Math.Hexes.Vectors.QRS;
using Akeldov.Math.Spatial2D;

namespace Akeldov.Math.Hexes.Tests.Chromatization;

public class HexFieldChromatizationTests
{
    [Test]
    public void Constructor_MatchesSingleHexChromaticClass_ForEveryLayout()
    {
        const int width = 5;
        const int height = 4;

        foreach (Layout layout in Enum.GetValues(typeof(Layout)))
        {
            HexFieldChromatization chromatization = new HexFieldChromatization(width, height, layout);

            Assert.That(chromatization.Width, Is.EqualTo(width));
            Assert.That(chromatization.Height, Is.EqualTo(height));
            Assert.That(chromatization.Layout, Is.EqualTo(layout));
            Assert.That(chromatization.ChromaticIndices, Has.Length.EqualTo(width * height));

            for (int y = 0; y < height; y++)
            {
                int rowStart = y * width;

                for (int x = 0; x < width; x++)
                {
                    byte expected = (byte)new VectorXYInt(x, y).GetChromaticClass(layout);

                    Assert.That(chromatization.ChromaticIndices[rowStart + x], Is.EqualTo(expected));
                }
            }
        }
    }

    [Test]
    public void ToHexFieldChromatization_UsesConstructor()
    {
        var resolution = new VectorXYInt(2, 1);

        HexFieldChromatization chromatization = resolution.ToHexFieldChromatization(Layout.OddR);

        Assert.That(chromatization.Width, Is.EqualTo(2));
        Assert.That(chromatization.Height, Is.EqualTo(1));
        Assert.That(chromatization.Layout, Is.EqualTo(Layout.OddR));
        Assert.That(chromatization.ChromaticIndices, Has.Length.EqualTo(2));
    }

    [Test]
    public void Constructor_WhenWidthIsNegative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new HexFieldChromatization(-1, 1, Layout.OddR));
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
