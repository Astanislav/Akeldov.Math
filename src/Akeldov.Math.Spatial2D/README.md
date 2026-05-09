# Akeldov.Math.Spatial2D

Akeldov.Math.Spatial2D is a small .NET library for two-dimensional geometry and spatial math.

It includes:

- float and integer 2D vectors
- lines, rays, segments, circles, and arcs
- curve projection and intersection helpers
- Voronoi partitioning
- Poisson disk point sampling
- influence fields and influence samplers

## Installation

```powershell
dotnet add package Akeldov.Math.Spatial2D
```

## Vectors

```csharp
using Akeldov.Math.Spatial2D;

var a = new VectorXY(3f, 4f);
var b = new VectorXY(1f, 2f);

VectorXY sum = a + b;
float length = a.Length;
float dot = VectorXY.Dot(a, b);
float distance = a.Distance(b);
```

## Curves

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

## Poisson Disk Sampling

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;

var sampler = new PoissonDiskPointSampler(new Random(12345), maxAttempts: 30);
var samples = sampler.Sample(new VectorXY(100f, 100f), minimalDistance: 8f);

foreach (var sample in samples)
{
    VectorXY point = sample.Point;
}
```

## Target Frameworks

The package targets:

- .NET Standard 2.1
- .NET 6.0

## License

MIT
