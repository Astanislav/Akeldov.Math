# Regions

`Region` represents a filled area defined by one or more closed contours.
It lives in the `Akeldov.Math.Spatial2D.Regions` namespace.

Contours describe boundaries. Regions describe area membership.
Use `Contour.Encloses` for a single boundary and `Region.Contains` for the filled area.

## Square With Hole

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Regions;

var region = new Region(new IContour[]
{
    CreateSquareContour(0f, 0f, 4f, 4f),
    CreateSquareContour(1f, 1f, 3f, 3f)
});

bool isInsideOuterArea = region.Contains(new PointXY(0.5f, 0.5f));
bool isInsideHole = region.Contains(new PointXY(2f, 2f));

static Contour CreateSquareContour(float left, float bottom, float right, float top)
{
    return new Contour(new IFinitePath[]
    {
        new ParameterizedSegment(new PointXY(left, bottom), new PointXY(right, bottom)),
        new ParameterizedSegment(new PointXY(right, bottom), new PointXY(right, top)),
        new ParameterizedSegment(new PointXY(right, top), new PointXY(left, top)),
        new ParameterizedSegment(new PointXY(left, top), new PointXY(left, bottom))
    });
}
```

`isInsideOuterArea` is `true`. `isInsideHole` is `false`.

## Fill Rule

Regions currently use `FillRule.EvenOdd`.
A point is inside the region when it lies inside an odd number of contours.
This supports holes and nested contours.

Region contours must not intersect or touch each other.
