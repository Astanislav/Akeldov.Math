# Poisson Disk Sampling

`PoissonDiskPointSampler` generates points in a rectangular 2D field while keeping a minimum distance between accepted samples.

## Constant Minimal Distance

```csharp
using Akeldov.Math.Spatial2D;
using Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk;

var sampler = new PoissonDiskPointSampler(new Random(12345), maxAttempts: 30);

IReadOnlyList<PoissonDiskPointSample> samples =
    sampler.Sample(new VectorXY(100f, 100f), minimalDistance: 8f);

foreach (var sample in samples)
{
    VectorXY point = sample.Point;
    float distance = sample.MinimalDistance;
}
```

## Field-Based Minimal Distance

You can also pass an `IFloatField` to vary spacing over the field.

```csharp
using Akeldov.Math.Spatial2D.Fields;

IFloatField distanceField = new ConstantFloatField(8f);

var samples = sampler.Sample(new VectorXY(100f, 100f), distanceField);
```

The field must return positive values. If it returns zero or a negative distance for a sampled point, sampling fails with an exception.

## Tuning

`maxAttempts` controls how many candidates are tried around an active point before the sampler retires that point. Higher values can produce denser point sets, but take more work.

