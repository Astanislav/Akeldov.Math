# Influence Fields

Influence fields sample values from positioned sources. They are useful for heat maps, procedural masks, control maps, and other continuous 2D value fields.

## Point Sources

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Fields;

var sources = new[]
{
    new FloatPointInfluenceSource(power: 1f, center: new VectorXY(0f, 0f), value: 0f),
    new FloatPointInfluenceSource(power: 1f, center: new VectorXY(10f, 0f), value: 100f)
};
```

## Inverse-Distance Weighted Sampling

```csharp
var sampler = new InverseDistanceWeightedFloatSampler<FloatPointInfluenceSource>();
var field = new InfluenceField<FloatPointInfluenceSource, float>(sampler, sources);

float value = field.Sample(new VectorXY(5f, 0f));
```

When the sampled point is very close to a source, the sampler returns that source value directly.

## Other Sampling Strategies

Spatial2D also includes:

- nearest-source samplers
- inverse-distance-weighted samplers
- barycentric samplers
- source cullers such as half-plane and Delaunay cullers

These pieces can be combined with `InfluenceField<TSource, TValue>` to trade off smoothness, locality, and performance.

