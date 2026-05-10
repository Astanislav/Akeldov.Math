# Curves

The curves package contains basic 2D primitives:

- `Line`
- `Ray`
- `Segment`
- `Circle`
- `Arc`

These types support distance, projection, and intersection-style workflows.

## Segment Projection

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var segment = new Segment(
    new VectorXY(0f, 0f),
    new VectorXY(10f, 0f));

var projection = segment.Project(new VectorXY(4f, 3f));

VectorXY point = projection.Point;
float distance = projection.Distance;
```

## Lines and Rays

```csharp
var line = new Line(
    new VectorXY(0f, 0f),
    new VectorXY(10f, 0f));

var ray = new Ray(VectorXY.Zero, angleRad: 0f);

var lineProjection = line.Project(new VectorXY(4f, 3f));
var rayProjection = ray.Project(new VectorXY(4f, 3f));
```

## Circles and Arcs

```csharp
var circle = new Circle(VectorXY.Zero, radius: 5f);
var arc = new Arc(VectorXY.Zero, radius: 5f, startAngleRad: 0f, stopAngleRad: MathF.PI);

float circleDistance = circle.Distance(new VectorXY(3f, 0f));
var arcProjection = arc.Project(new VectorXY(0f, 8f));
```

## Helpers

Curve extension methods include:

- `IsInsideContour` for closed contours.
- `IsOnTheArc` for point-to-arc checks.
- `Shorten` and `Extend` for segments.
- `PerpendicularAt` and `IsSameSide` for lines.
- `CreateArcInAngle` and `CreateIncircleInAngle` for corner construction.
