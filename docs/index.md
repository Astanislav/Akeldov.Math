# Akeldov.Math

Akeldov.Math is a collection of small .NET math libraries.

## Libraries

### Akeldov.Math.Spatial2D

Two-dimensional geometry and spatial math utilities:

- float and integer 2D vectors
- lines, rays, segments, circles, and arcs
- curve projection and intersection helpers
- Voronoi partitioning
- Poisson disk point sampling
- influence fields and influence samplers

[Open Spatial2D documentation](Spatial2D/)

### Akeldov.Math.Intervals

Interval primitives for numeric ranges.

[Open Intervals documentation](Intervals/)

## Repository

The repository keeps related libraries in one solution:

```text
src/
  Akeldov.Math.Spatial2D/
  Akeldov.Math.Intervals/
tests/
  Akeldov.Math.Spatial2D.Tests/
docs/
  Spatial2D/
  Intervals/
```

## Documentation

This site is built with MkDocs.

```powershell
mkdocs serve
```

```powershell
mkdocs build
```
