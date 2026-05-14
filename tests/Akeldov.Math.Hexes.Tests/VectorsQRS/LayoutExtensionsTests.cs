using Akeldov.Math.Hexes.Vectors.QRS;

namespace Akeldov.Math.Hexes.Tests.VectorsQRS;

public class LayoutExtensionsTests
{
    [TestCase(Layout.OddR, true, false)]
    [TestCase(Layout.EvenR, true, false)]
    [TestCase(Layout.OddQ, false, true)]
    [TestCase(Layout.EvenQ, false, true)]
    public void OrientationChecks_ReturnExpectedValues(Layout layout, bool expectedPointy, bool expectedFlat)
    {
        Assert.Multiple(() =>
        {
            Assert.That(layout.IsPointyTop(), Is.EqualTo(expectedPointy));
            Assert.That(layout.IsFlatTop(), Is.EqualTo(expectedFlat));
        });
    }
}
