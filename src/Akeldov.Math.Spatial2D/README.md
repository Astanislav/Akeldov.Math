# Akeldov.Math.Spatial2D

Akeldov.Math.Spatial2D is a .NET library for practical two-dimensional geometry, spatial sampling, partitioning, and value fields.

## Features

The library provides:

- Float and integer vector types: `VectorXY` and `VectorXYInt`.
- Curve primitives: `Line`, `Ray`, `Segment`, `Circle`, and `Arc`.
- Projection, distance, intersection, angle, contour, and centroid helpers.
- Poisson disk point sampling with constant or spatially varying minimal distance.
- Weighted Voronoi partitioning for positioned items.
- Influence fields for sampling values from point or curve sources.
- Source culling and interpolation strategies for local field behavior.

## Quick Example

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Curves;

var segment = new Segment(
    new VectorXY(0f, 0f),
    new VectorXY(10f, 0f));

var projection = segment.Project(new VectorXY(4f, 3f));

VectorXY closestPoint = projection.Point;
float distance = projection.Distance;
```

## Documentation

Documentation is available at:

[https://akeldov.github.io/Akeldov.Math/Spatial2D/](https://akeldov.github.io/Akeldov.Math/Spatial2D/)
