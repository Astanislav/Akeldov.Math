# Getting Started

## Install

```powershell
dotnet add package Akeldov.Math.Spatial2D
```

## Namespaces

Most core primitives live in `Akeldov.Math.Spatial2D`.

```csharp
using Akeldov.Math.Spatial2D;
```

Feature areas use sub-namespaces:

```csharp
using Akeldov.Math.Spatial2D.Contours;
using Akeldov.Math.Spatial2D.Curves;
using Akeldov.Math.Spatial2D.Fields;
using Akeldov.Math.Spatial2D.Partitioning.Voronoi;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;
```

## Coordinates

The library uses `float` for `VectorXY` and `int` for `VectorXYInt`.

```csharp
var position = new VectorXY(3.5f, 8f);
var cell = new VectorXYInt(3, 8);

VectorXY asFloat = cell;
VectorXYInt truncated = (VectorXYInt)position;
VectorXYInt rounded = position.RoundToInt();
```

## Floating-Point Comparisons

Geometry operations often need tolerance-based comparison. Use `AlmostEquals` and `GeometryConstants.GeometryEpsilon` when exact component equality is too strict.

```csharp
var a = new VectorXY(1f, 2f);
var b = new VectorXY(1f + GeometryConstants.GeometryEpsilon / 2f, 2f);

bool closeEnough = a.AlmostEquals(b);
```
