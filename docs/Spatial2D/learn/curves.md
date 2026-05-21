# Curves

The curves package contains basic 2D primitives:

- `Line`
- `ParametricLine`
- `Ray`
- `Segment`
- `Circle`
- `Arc`

These types support distance, projection, and intersection-style workflows.
`Project` returns the projected point and distance. Curves with a length-based parameterization also expose
`ProjectWithParameter`, which additionally returns the curve coordinate.

Angles are expressed in radians by default. Non-radian members use an explicit suffix, such as `Deg`, and document their unit.

## Segment Projection

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;

var segment = new ParameterizedSegment(
    new PointXY(0f, 0f),
    new PointXY(10f, 0f));

var projection = segment.ProjectWithParameter(new PointXY(4f, 3f));

PointXY point = projection.ProjectedPoint;
float curveCoordinate = projection.CurveCoordinate;
float distance = projection.Distance;
```

## Lines and Rays

```csharp
var line = new Line(
    new PointXY(0f, 0f),
    new PointXY(10f, 0f));

var parametricLine = new ParametricLine(
    line,
    referencePoint: new PointXY(2f, 0f));

var ray = new Ray(new PointXY(0f, 0f), angle: 0f);

var lineProjection = line.Project(new PointXY(4f, 3f));
var parametricLineProjection = parametricLine.ProjectWithParameter(new PointXY(4f, 3f));
var rayProjection = ray.ProjectWithParameter(new PointXY(4f, 3f));
```

## Circles and Arcs

```csharp
var circle = new Circle(new PointXY(0f, 0f), radius: 5f);
var arc = new Arc(new PointXY(0f, 0f), radius: 5f, startAngle: 0f, endAngle: MathF.PI);

float circleDistance = circle.Distance(new PointXY(3f, 0f));
var arcProjection = arc.Project(new PointXY(0f, 8f));
bool isWithinAngularRegion = arc.IsWithinAngularRegion(new PointXY(1f, 1f));
```

## Contours

Contours are closed boundaries made from bounded parameterized curves and live in the `Akeldov.Math.Spatial2D.Contours` namespace.
Each curve must continue from the previous curve, and the final curve must close the contour.

```csharp
var contour = new Contour(new IFinitePath[]
{
    new ParameterizedArc(
        new PointXY(0f, 0f),
        radius: 5f,
        startAngle: 0f,
        endAngle: 2f * MathF.PI,
        angularDirection: AngularDirection.Counterclockwise)
});

bool isInside = contour.Encloses(new PointXY(3f, 0f));
```

## Helpers

Curve extension methods include:

- `Shorten` and `Extend` for segments.
- `PerpendicularAt` and `IsSameSide` for lines.
- `CreateFilletArc` and `CreateCornerTangentCircle` for corner construction.
