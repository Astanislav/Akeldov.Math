using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Regions;

namespace Akeldov.Math.Spatial2D.Tests.Regions;

public class RegionTests
{
    [Test]
    public void Constructor_WhenContoursIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Region(null!));
    }

    [Test]
    public void Constructor_WhenContoursIsEmpty_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Region(Array.Empty<IContour>()));
    }

    [Test]
    public void Constructor_WhenContoursContainsNull_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Region(new IContour[] { null! }));
    }

    [Test]
    public void Constructor_WhenFillRuleIsUnsupported_Throws()
    {
        var contour = CreateSquareContour(0f, 0f, 1f, 1f);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Region(new IContour[] { contour }, (FillRule)42));

        Assert.That(exception!.ParamName, Is.EqualTo("fillRule"));
    }

    [Test]
    public void Constructor_WhenContoursIntersect_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
            new Region(new IContour[]
            {
                CreateSquareContour(0f, 0f, 4f, 4f),
                CreateSquareContour(2f, -1f, 5f, 2f)
            }));
    }

    [Test]
    public void Constructor_WhenContoursTouch_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
            new Region(new IContour[]
            {
                CreateSquareContour(0f, 0f, 4f, 4f),
                CreateSquareContour(4f, 1f, 5f, 3f)
            }));
    }

    [Test]
    public void Contours_WhenAccessed_ReturnsReadOnlyView()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 1f, 1f)
        });

        Assert.That(region.Contours, Is.Not.InstanceOf<IContour[]>());
        Assert.Throws<NotSupportedException>(() =>
            ((IList<IContour>)region.Contours)[0] = CreateSquareContour(1f, 1f, 2f, 2f));
    }

    [Test]
    public void Contains_WhenPointIsInsideOuterContourAndOutsideHole_ReturnsTrue()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(0.5f, 0.5f)), Is.True);
    }

    [Test]
    public void Contains_WhenPointIsInsideHole_ReturnsFalse()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(2f, 2f)), Is.False);
    }

    [Test]
    public void Contains_WhenPointIsOnHoleBoundary_ReturnsFalse()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(1f, 2f)), Is.False);
    }

    [Test]
    public void Contains_WhenPointIsWithinCustomGeometryEpsilonOfBoundary_ReturnsTrue()
    {
        IRegion region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 1f, 1f)
        });

        var point = new VectorXY(-0.0005f, 0.5f);

        Assert.That(region.Contains(point), Is.False);
        Assert.That(region.Contains(point, 0.001f), Is.True);
    }

    [Test]
    public void Contains_PassesGeometryEpsilonToContours()
    {
        var contour = new EpsilonAwareContour();
        IRegion region = new Region(new IContour[] { contour });

        bool contains = region.Contains(VectorXY.Zero, 0.25f);

        Assert.That(contains, Is.True);
        Assert.That(contour.LastGeometryEpsilon, Is.EqualTo(0.25f));
    }

    [Test]
    public void Contains_WhenRegionIsSquareWithSquareHole_ClassifiesPoints()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 4f, 4f),
            CreateSquareContour(1f, 1f, 3f, 3f)
        });

        Assert.That(region.Contains(new VectorXY(-0.5f, 2f)), Is.False);
        Assert.That(region.Contains(new VectorXY(0.5f, 0.5f)), Is.True);
        Assert.That(region.Contains(new VectorXY(2f, 2f)), Is.False);
        Assert.That(region.Contains(new VectorXY(0f, 2f)), Is.True);
        Assert.That(region.Contains(new VectorXY(1f, 2f)), Is.False);
    }

    [Test]
    public void Contains_WhenContoursAreNested_AlternatesInsideAndOutside()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 8f, 8f),
            CreateSquareContour(1f, 1f, 7f, 7f),
            CreateSquareContour(2f, 2f, 6f, 6f),
            CreateSquareContour(3f, 3f, 5f, 5f)
        });

        Assert.That(region.Contains(new VectorXY(0.5f, 0.5f)), Is.True);
        Assert.That(region.Contains(new VectorXY(1.5f, 1.5f)), Is.False);
        Assert.That(region.Contains(new VectorXY(2.5f, 2.5f)), Is.True);
        Assert.That(region.Contains(new VectorXY(4f, 4f)), Is.False);
        Assert.That(region.Contains(new VectorXY(8.5f, 8.5f)), Is.False);
    }

    [Test]
    public void Contains_WhenPointCoordinateIsInvalid_Throws()
    {
        var region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 1f, 1f)
        });

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            region.Contains(new VectorXY(float.NaN, 0f)));

        Assert.That(exception!.ParamName, Is.EqualTo("point"));
    }

    [TestCase(-1e-6f)]
    [TestCase(float.NaN)]
    [TestCase(float.PositiveInfinity)]
    [TestCase(float.NegativeInfinity)]
    public void Contains_WhenGeometryEpsilonIsInvalid_Throws(float geometryEpsilon)
    {
        IRegion region = new Region(new IContour[]
        {
            CreateSquareContour(0f, 0f, 1f, 1f)
        });

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            region.Contains(VectorXY.Zero, geometryEpsilon));

        Assert.That(exception!.ParamName, Is.EqualTo("geometryEpsilon"));
    }

    private static Contour CreateSquareContour(float left, float bottom, float right, float top)
    {
        return new Contour(new IFinitePath[]
        {
            new ParameterizedSegment(new VectorXY(left, bottom), new VectorXY(right, bottom)),
            new ParameterizedSegment(new VectorXY(right, bottom), new VectorXY(right, top)),
            new ParameterizedSegment(new VectorXY(right, top), new VectorXY(left, top)),
            new ParameterizedSegment(new VectorXY(left, top), new VectorXY(left, bottom))
        });
    }

    private sealed class EpsilonAwareContour : IContour
    {
        private static readonly IFinitePath[] ContourCurves =
        {
            new DistantBoundaryCurve()
        };

        public float LastGeometryEpsilon { get; private set; }

        public IReadOnlyList<IFinitePath> Curves => ContourCurves;

        public bool Encloses(
            VectorXY point,
            float geometryEpsilon = GeometryConstants.GeometryEpsilon)
        {
            LastGeometryEpsilon = geometryEpsilon;
            return geometryEpsilon == 0.25f;
        }

        public float Distance(VectorXY point)
        {
            float minDistance = float.MaxValue;

            for (int i = 0; i < Curves.Count; i++)
            {
                float distance = Curves[i].Distance(point);
                if (distance < minDistance)
                    minDistance = distance;
            }

            return minDistance;
        }

        public float SignedDistance(VectorXY point, float geometryEpsilon = GeometryConstants.GeometryEpsilon)
        {
            float distance = Distance(point);
            return Encloses(point, geometryEpsilon) ? -distance : distance;
        }
    }

    private sealed class DistantBoundaryCurve : IFinitePath
    {
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
            return new List<VectorXY>();
        }

        public float Distance(VectorXY point) => 1f;

        public CurveProjection Project(VectorXY point) => new(VectorXY.Zero, Distance(point));

        public ParameterizedCurveProjection ProjectWithParameter(VectorXY point) => new(
            VectorXY.Zero,
            0f,
            Distance(point));
    }
}
