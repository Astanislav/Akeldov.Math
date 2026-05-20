using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;

namespace Akeldov.Math.Spatial2D.Tests.Curves;

public class ContourTests
{
    [Test]
    public void Constructor_WhenCurvesIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Contour(null!));
    }

    [Test]
    public void Constructor_WhenCurvesIsEmpty_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Contour(Array.Empty<IFinitePath>()));
    }

    [Test]
    public void Constructor_WhenCurvesContainsNull_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Contour(new IFinitePath[] { null! }));
    }

    [Test]
    public void Constructor_WhenCurvesAreDisconnected_Throws()
    {
        var exception = Assert.Throws<ArgumentException>(() => new Contour(new IFinitePath[]
        {
            new ParameterizedSegment(new VectorXY(0f, 0f), new VectorXY(1f, 0f)),
            new ParameterizedSegment(new VectorXY(2f, 0f), new VectorXY(2f, 1f)),
            new ParameterizedSegment(new VectorXY(2f, 1f), new VectorXY(0f, 0f))
        }));

        Assert.That(exception!.ParamName, Is.EqualTo("curves"));
    }

    [Test]
    public void Constructor_WhenCurvesDoNotClose_Throws()
    {
        var exception = Assert.Throws<ArgumentException>(() => new Contour(new IFinitePath[]
        {
            new ParameterizedSegment(new VectorXY(0f, 0f), new VectorXY(1f, 0f)),
            new ParameterizedSegment(new VectorXY(1f, 0f), new VectorXY(1f, 1f)),
            new ParameterizedSegment(new VectorXY(1f, 1f), new VectorXY(0f, 1f))
        }));

        Assert.That(exception!.ParamName, Is.EqualTo("curves"));
    }

    [Test]
    public void Curves_WhenAccessed_ReturnsReadOnlyView()
    {
        var contour = new Contour(new IFinitePath[]
        {
            new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI)
        });

        Assert.That(contour.Curves, Is.Not.InstanceOf<IFinitePath[]>());
        Assert.Throws<NotSupportedException>(() =>
            ((IList<IFinitePath>)contour.Curves)[0] = new ParameterizedSegment(VectorXY.Zero, VectorXY.One));
    }

    [Test]
    public void Encloses_WhenPointIsInsideSegmentContour_ReturnsTrue()
    {
        var contour = new Contour(new IFinitePath[]
        {
            new ParameterizedSegment(new VectorXY(0f, 0f), new VectorXY(2f, 0f)),
            new ParameterizedSegment(new VectorXY(2f, 0f), new VectorXY(2f, 2f)),
            new ParameterizedSegment(new VectorXY(2f, 2f), new VectorXY(0f, 2f)),
            new ParameterizedSegment(new VectorXY(0f, 2f), new VectorXY(0f, 0f))
        });

        Assert.That(contour.Encloses(new VectorXY(1f, 1f)), Is.True);
    }

    [Test]
    public void Encloses_WhenPointIsOutsideContour_ReturnsFalse()
    {
        IContour contour = new Contour(new IFinitePath[]
        {
            new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI)
        });

        bool isInside = contour.Encloses(new VectorXY(2f, 0f));

        Assert.That(isInside, Is.False);
    }

    [Test]
    public void Encloses_WhenPointIsOnContour_ReturnsTrue()
    {
        var contour = new Contour(new IFinitePath[]
        {
            new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI)
        });

        Assert.That(contour.Encloses(new VectorXY(1f, 0f)), Is.True);
    }

    [Test]
    public void Encloses_WhenPointIsWithinCustomGeometryEpsilonOfContour_ReturnsTrue()
    {
        IContour contour = new Contour(new IFinitePath[]
        {
            new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI)
        });

        var point = new VectorXY(1.0005f, 0f);

        Assert.That(contour.Encloses(point), Is.False);
        Assert.That(contour.Encloses(point, 0.001f), Is.True);
    }

    [Test]
    public void Encloses_PassesGeometryEpsilonToCurveRayIntersections()
    {
        var curve = new EpsilonAwareCurve();
        IContour contour = new Contour(new IFinitePath[] { curve });

        bool encloses = contour.Encloses(VectorXY.Zero, 0.25f);

        Assert.That(encloses, Is.True);
        Assert.That(curve.LastGeometryEpsilon, Is.EqualTo(0.25f));
    }

    [Test]
    public void Encloses_WhenPointCoordinateIsInvalid_Throws()
    {
        var contour = new Contour(new IFinitePath[]
        {
            new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI)
        });

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            contour.Encloses(new VectorXY(float.NaN, 0f)));

        Assert.That(exception!.ParamName, Is.EqualTo("point"));
    }

    [TestCase(-1e-6f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Encloses_WhenGeometryEpsilonIsInvalid_Throws(float geometryEpsilon)
    {
        IContour contour = new Contour(new IFinitePath[]
        {
            new Arc(VectorXY.Zero, 1f, 0f, 2f * MathF.PI)
        });

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            contour.Encloses(VectorXY.Zero, geometryEpsilon));

        Assert.That(exception!.ParamName, Is.EqualTo("geometryEpsilon"));
    }

    private sealed class EpsilonAwareCurve : IFinitePath
    {
        public float LastGeometryEpsilon { get; private set; }

        public VectorXY StartPoint => VectorXY.Zero;

        public VectorXY EndPoint => VectorXY.Zero;

        public VectorXY EndpointA => StartPoint;

        public VectorXY EndpointB => EndPoint;

        public float Length => 0f;

        public VectorXY GetPoint(float curveCoordinate) => VectorXY.Zero;

        public List<VectorXY> GetRayIntersections(
            Ray ray,
            float geometryEpsilon = GeometryConstants.GeometryEpsilon)
        {
            LastGeometryEpsilon = geometryEpsilon;

            return geometryEpsilon.AlmostEquals(0.25f)
                ? new List<VectorXY> { new VectorXY(1f, 0f) }
                : new List<VectorXY>();
        }

        public float Distance(VectorXY point) => 1f;

        public CurveProjection Project(VectorXY point) => new(VectorXY.Zero, Distance(point));

        public ParameterizedCurveProjection ProjectWithParameter(VectorXY point) => new(
            VectorXY.Zero,
            0f,
            Distance(point));
    }
}
