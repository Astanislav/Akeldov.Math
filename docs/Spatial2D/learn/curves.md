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

var segment = new Segment(
    new VectorXY(0f, 0f),
    new VectorXY(10f, 0f));

var projection = segment.ProjectWithParameter(new VectorXY(4f, 3f));

VectorXY point = projection.ProjectedPoint;
float curveCoordinate = projection.CurveCoordinate;
float distance = projection.Distance;
```

## Lines and Rays

```csharp
var line = new Line(
    new VectorXY(0f, 0f),
    new VectorXY(10f, 0f));

var parametricLine = new ParametricLine(
    line,
    referencePoint: new VectorXY(2f, 0f));

var ray = new Ray(VectorXY.Zero, angle: 0f);

var lineProjection = line.Project(new VectorXY(4f, 3f));
var parametricLineProjection = parametricLine.ProjectWithParameter(new VectorXY(4f, 3f));
var rayProjection = ray.ProjectWithParameter(new VectorXY(4f, 3f));
```

## Circles and Arcs

```csharp
var circle = new Circle(VectorXY.Zero, radius: 5f);
var arc = new Arc(VectorXY.Zero, radius: 5f, startAngle: 0f, endAngle: MathF.PI);

float circleDistance = circle.Distance(new VectorXY(3f, 0f));
var arcProjection = arc.ProjectWithParameter(new VectorXY(0f, 8f));
bool isWithinAngularRegion = arc.IsWithinAngularRegion(new VectorXY(1f, 1f));
```

## Contours

Contours are closed boundaries made from bounded parameterized curves and live in the `Akeldov.Math.Spatial2D.Contours` namespace.

```csharp
var contour = new Contour(new IBoundedParameterizedCurve[]
{
    new Arc(VectorXY.Zero, radius: 5f, startAngle: 0f, endAngle: 2f * MathF.PI)
});

bool isInside = contour.Contains(new VectorXY(3f, 0f));
```

## Helpers

Curve extension methods include:

- `Shorten` and `Extend` for segments.
- `PerpendicularAt` and `IsSameSide` for lines.
- `CreateFilletArc` and `CreateCornerTangentCircle` for corner construction.
