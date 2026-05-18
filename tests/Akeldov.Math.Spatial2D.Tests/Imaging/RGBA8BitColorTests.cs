using Akeldov.Math.Spatial2D.Imaging;

namespace Akeldov.Math.Spatial2D.Tests.Imaging;

public class RGBA8BitColorTests
{
    [Test]
    public void Constructor_WhenValuesAreProvided_StoresChannels()
    {
        var color = new RGBA8BitColor(1, 2, 3, 4);

        Assert.That(color.Red, Is.EqualTo(1));
        Assert.That(color.Green, Is.EqualTo(2));
        Assert.That(color.Blue, Is.EqualTo(3));
        Assert.That(color.Alpha, Is.EqualTo(4));
    }

    [Test]
    public void EqualityMembers_CompareChannelValues()
    {
        var first = new RGBA8BitColor(1, 2, 3, 4);
        var second = new RGBA8BitColor(1, 2, 3, 4);
        var third = new RGBA8BitColor(1, 2, 3, 5);

        Assert.That(first, Is.EqualTo(second));
        Assert.That(first == second, Is.True);
        Assert.That(first != third, Is.True);
        Assert.That(first.GetHashCode(), Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void ToString_ReturnsChannelValues()
    {
        var color = new RGBA8BitColor(1, 2, 3, 4);

        Assert.That(color.ToString(), Is.EqualTo("rgba8(1, 2, 3, 4)"));
    }
}
