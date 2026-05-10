# Voronoi Partitioning

`VoronoiPartitioner<TItem>` assigns positioned items to the closest configured site.

Items must implement `IHasPosition2D`.

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Partitioning.Voronoi;

public sealed class MapCell : IHasPosition2D
{
    public MapCell(string id, VectorXY center)
    {
        Id = id;
        Center = center;
    }

    public string Id { get; }
    public VectorXY Center { get; }
}
```

## Basic Partitioning

```csharp
var sites = new[]
{
    new Site(new VectorXY(0f, 0f), power: 1f),
    new Site(new VectorXY(10f, 0f), power: 1f)
};

var items = new[]
{
    new MapCell("left", new VectorXY(1f, 0f)),
    new MapCell("right", new VectorXY(9f, 0f))
};

var partitioner = new VoronoiPartitioner<MapCell>(sites);
IReadOnlyList<VoronoiCell<MapCell>> cells = partitioner.Partition(items);
```

## Weighted Sites

Each `Site` has a `Power`. Higher power affects the closest-site calculation and can pull items toward that site.

```csharp
var weightedSites = new[]
{
    new Site(new VectorXY(0f, 0f), power: 1f),
    new Site(new VectorXY(10f, 0f), power: 3f)
};
```

## Empty Cell Policy

Use `EmptyCellPolicy` to choose how empty cells are handled:

- `ThrowException`
- `Exclude`
- `LeaveAsIs`

```csharp
var partitioner = new VoronoiPartitioner<MapCell>(
    sites,
    EmptyCellPolicy.LeaveAsIs);
```

The partitioner also supports relaxation iterations for centroid-style refinement.

