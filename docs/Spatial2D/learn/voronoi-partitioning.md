# Voronoi Partitioning

`VoronoiPartitioner<TItem>` assigns positioned items to the closest configured site.

Items must implement `IHasPosition2D`.

```csharp
using Akeldov.Math.Spatial2D;

public sealed class MapCell : IHasPosition2D
{
    public MapCell(string id, VectorXY position)
    {
        Id = id;
        Position = position;
    }

    public string Id { get; }
    public VectorXY Position { get; }
}
```

## Equal Site Weights

Use equal weights when each site should compete by distance alone.

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Partitioning.Voronoi;

var sites = new[]
{
    new Site(new VectorXY(25f, 30f), weight: 1f),
    new Site(new VectorXY(95f, 30f), weight: 1f)
};

var partitioner = new VoronoiPartitioner<MapCell>(
    sites,
    EmptyCellPolicy.LeaveAsIs);

IReadOnlyList<VoronoiCell<MapCell>> cells = partitioner.Partition(items);
```

![Voronoi partition with equal site weights](../../assets/spatial2d/voronoi/equal-site-weights.svg)

## Weighted Sites

Increase a site's `Weight` to let it claim a larger region.

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Partitioning.Voronoi;

var sites = new[]
{
    new Site(new VectorXY(25f, 30f), weight: 1f),
    new Site(new VectorXY(95f, 30f), weight: 2f)
};

var partitioner = new VoronoiPartitioner<MapCell>(
    sites,
    EmptyCellPolicy.LeaveAsIs);

IReadOnlyList<VoronoiCell<MapCell>> cells = partitioner.Partition(items);
```

![Voronoi partition with weighted sites](../../assets/spatial2d/voronoi/weighted-sites.svg)

## Weight Edge Cases

At least one site must have positive weight. A zero-weight site only receives items that are located at that site position.

If an item is located at a site position, that site is selected before any weighted-distance comparison. If no site contains the item and one or more sites have `float.PositiveInfinity` weight, the nearest infinite-weight site is selected. Otherwise, sites compete by squared distance divided by squared weight.

## Empty Cell Policy

Use `EmptyCellPolicy` to choose how empty cells are handled:

- `ThrowException`
- `Exclude`
- `LeaveAsIs`
