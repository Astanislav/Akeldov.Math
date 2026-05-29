# Circular Curves

Circular curves describe round geometry in 2D space: full circumferences and bounded angular spans.
They live in the `Akeldov.Math.Spatial2D.Curves` namespace and share the same projection, distance, and ray-intersection conventions as the [curves overview](../curves.md).

Angles are expressed in radians by default.
Curve coordinates and distances are measured in world coordinate units.
Degree-based members use the `Deg` suffix.

<p>
  <img alt="Circle distance raster" src="../../../../assets/spatial2d/curves/circle-distance.png" width="160">
  <img alt="Arc distance raster" src="../../../../assets/spatial2d/curves/arc-distance.png" width="160">
  <img alt="Parameterized arc distance raster" src="../../../../assets/spatial2d/curves/parameterized-arc-distance.png" width="160">
  <img alt="Parameterized arc with growing thickness" src="../../../../assets/spatial2d/curves/parameterized-arc-growing-thickness.png" width="160">
</p>

| Type | Geometry | Coordinate Domain | Use When |
|---|---|---|---|
| `Circle` | Full circumference | - | You need distance or projection to a ring, not a filled disk. |
| `Arc` | Bounded circular span | - | You need geometric distance, projection, or intersections for part of a circle. |
| `ParameterizedArc` | Bounded directed circular span | `[0, Length]` | You need traversal direction or distance along the arc. |

## Choosing a Type

Use `Circle` when the whole circumference is the target geometry.
`Circle.Distance` measures the shortest distance to the circumference, so points inside the circle still measure outward to the ring.

Use `Arc` when endpoint order should describe only the geometric angular region.
`Arc` follows the positive angular span from `StartAngle` to `EndAngle`, and has no curve coordinate.

Use `ParameterizedArc` when you need a path coordinate.
`ProjectWithParameter` returns the closest point, the distance to the curve, and the length coordinate of that point along the selected `AngularDirection`.
`GetPoint` does the inverse: it returns a point at a specific curve coordinate.

## Circles

`Circle` represents a full circumference with a `Center`, `Radius`, and `Length`.
The radius must be finite and non-negative.

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var circle = new Circle(
    center: new PointXY(1f, 1f),
    radius: 2f);

CurveProjection projection = circle.Project(new PointXY(4f, 1f));

PointXY closestPoint = projection.ProjectedPoint; // (3, 1)
float distance = projection.Distance;             // 1
float circumference = circle.Length;              // 4 * PI
```

If the projected point is exactly at the circle center, projection uses the point on the positive X axis.
If the radius is zero, projection returns the center.

## Arcs

`Arc` represents a bounded part of a circle.
The `startAngle` and `endAngle` constructor arguments, plus the `StartAngle` and `EndAngle` properties, are in radians.
Stored angles are normalized.

```csharp
using System;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var arc = new Arc(
    center: new PointXY(0f, 0f),
    radius: 5f,
    startAngle: 0f,
    endAngle: MathF.PI / 2f);

bool isInsideArcAngle = arc.IsWithinAngularRegion(new PointXY(1f, 1f));
PointXY start = arc.StartPoint; // (5, 0)
PointXY end = arc.EndPoint;     // (0, 5)

CurveProjection projection = arc.Project(new PointXY(-3f, 4f));
```

When a point's direction from the center is inside the arc's angular region, projection lands on the source circle.
When the direction is outside the angular region, projection clamps to the nearest endpoint.

Equal input angles create a zero-length arc at the start point.
An end angle one full turn after the start angle creates a full circle, even though normalized start and end angles are equal.

```csharp
using System;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var zeroLength = new Arc(
    center: new PointXY(0f, 0f),
    radius: 1f,
    startAngle: 0f,
    endAngle: 0f);

var fullCircle = new Arc(
    center: new PointXY(0f, 0f),
    radius: 1f,
    startAngle: 0f,
    endAngle: 2f * MathF.PI);
```

## Parameterized Arcs

`ParameterizedArc` adds `AngularDirection` and a length-based curve coordinate.
The coordinate starts at `0` at `StartPoint` and ends at `Length` at `EndPoint`.

```csharp
using System;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var path = new ParameterizedArc(
    center: new PointXY(0f, 0f),
    radius: 2f,
    startAngle: 0f,
    endAngle: MathF.PI,
    angularDirection: AngularDirection.Counterclockwise);

PointXY halfway = path.GetPoint(path.Length * 0.5f); // (0, 2)

ParameterizedCurveProjection projection =
    path.ProjectWithParameter(new PointXY(0f, 3f));

PointXY projectedPoint = projection.ProjectedPoint; // (0, 2)
float curveCoordinate = projection.CurveCoordinate; // PI
float distance = projection.Distance;               // 1
Arc geometricArc = (Arc)path;
```

`AngularDirection.Counterclockwise` increases the angle from `StartAngle` toward `EndAngle`.
`AngularDirection.Clockwise` traverses the same start and end points in the opposite direction, so the angular region may be different.

```csharp
using System;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var clockwisePath = new ParameterizedArc(
    center: new PointXY(0f, 0f),
    radius: 1f,
    startAngle: 0f,
    endAngle: MathF.PI / 2f,
    angularDirection: AngularDirection.Clockwise);

bool includesLowerHalf = clockwisePath.IsWithinAngularRegion(new PointXY(0f, -1f));
PointXY pointAfterQuarterTurn = clockwisePath.GetPoint(MathF.PI / 2f);
```

`GetPoint` accepts coordinates from `0` through `Length`.
Coordinates outside that range, plus NaN or infinite coordinates, throw `ArgumentOutOfRangeException`.

`ParameterizedArc` exposes `StartAngleDeg`, `EndAngleDeg`, and `ToDegreesString()` when degree output is more convenient.

## Ray Intersections

`GetRayIntersections` returns a new mutable list of intersection points in the forward direction of the ray.
For arcs, intersections with the source circle are filtered to the arc's angular region.

```csharp
using System.Collections.Generic;
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var circle = new Circle(
    center: new PointXY(0f, 0f),
    radius: 2f);

var ray = new Ray(new PointXY(-4f, 0f));

List<PointXY> hits = circle.GetRayIntersections(ray);
```

Tangent rays return one point.
Rays starting inside a circle return only the forward exit point.
For zero-radius arcs, a ray through the center returns the center point.

The `geometryEpsilon` argument is measured in world coordinate units and controls comparisons near tangencies and endpoints.

## Helpers

`CornerExtensions` can construct circular geometry tangent to two sides of a corner.

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

PointXY firstSidePoint = new PointXY(1f, 0f);
PointXY vertex = new PointXY(0f, 0f);
PointXY secondSidePoint = new PointXY(0f, 1f);

Circle tangentCircle = CornerExtensions.CreateCornerTangentCircle(
    firstSidePoint,
    vertex,
    secondSidePoint,
    radius: 0.25f);

Arc filletArc = CornerExtensions.CreateFilletArc(
    firstSidePoint,
    vertex,
    secondSidePoint,
    radius: 0.25f);
```
