# Akeldov.Math.Spatial2D

Akeldov.Math.Spatial2D is a small .NET library for two-dimensional geometry and spatial math.

It focuses on practical building blocks for simulations, procedural generation, map processing, and geometry-heavy application code.

## Features

- `VectorXY` and `VectorXYInt` for float and integer 2D coordinates.
- Curve primitives: `Line`, `Ray`, `Segment`, `Circle`, and `Arc`.
- Projection, distance, intersection, and contour helpers.
- Poisson disk point sampling with constant or field-based minimal distance.
- Weighted Voronoi partitioning over positioned items.
- Influence fields with nearest, inverse-distance-weighted, and barycentric sampling strategies.

## Installation

```powershell
dotnet add package Akeldov.Math.Spatial2D
```

## Target Frameworks

- .NET Standard 2.1
- .NET 6.0

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

## Contents

- [Getting Started](getting-started.md)
- [Vectors](vectors.md)
- [Curves](curves.md)
- [Poisson Disk Sampling](poisson-disk-sampling.md)
- [Voronoi Partitioning](voronoi-partitioning.md)
- [Influence Fields](influence-fields.md)
