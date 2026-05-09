using Akeldov.Math.Spatial2D.Fields;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Sampling.Point.PoissonDisk
{
    public class PoissonDiskPointSampler
    {
        private readonly Random _random;
        private readonly int _maxAttempts;

        public PoissonDiskPointSampler(Random random, int maxAttempts)
        {
            if (maxAttempts <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxAttempts));

            _random = random ?? throw new ArgumentNullException(nameof(random));
            _maxAttempts = maxAttempts;
        }

        public IReadOnlyList<PoissonDiskPointSample> Sample(VectorXY fieldSize, float minimalDistance)
        {
            if (minimalDistance <= 0)
                throw new ArgumentOutOfRangeException(nameof(minimalDistance));

            return Sample(fieldSize, new ConstantFloatField(minimalDistance));
        }

        public IReadOnlyList<PoissonDiskPointSample> Sample(VectorXY fieldSize, IFloatField minimalDistanceField)
        {
            if (fieldSize.X <= 0 || fieldSize.Y <= 0)
                throw new ArgumentOutOfRangeException(nameof(fieldSize));

            if (minimalDistanceField == null)
                throw new ArgumentNullException(nameof(minimalDistanceField));

            if (minimalDistanceField.Min <= 0 || minimalDistanceField.Max <= 0)
                throw new ArgumentOutOfRangeException(nameof(minimalDistanceField));

            // The grid uses the maximum possible distance so every conflicting point must be in
            // the candidate cell or one of its immediate neighbours. Smaller cells require a
            // wider search radius to preserve the variable-distance invariant.
            float cellSize = minimalDistanceField.Max;

            var processList = new List<VectorXY>();
            var samples = new List<PoissonDiskPointSample>();
            var grid = new Grid(fieldSize, cellSize);

            var first = _random.SelectRandomPoint(fieldSize);
            processList.Add(first);
            var firstMinimalDistance = SampleMinimalDistance(minimalDistanceField, first);

            if (grid.TryAdd(first, firstMinimalDistance))
            {
                samples.Add(new PoissonDiskPointSample(first, firstMinimalDistance));
            }

            while (processList.Count > 0)
            {
                int index = _random.Next(processList.Count);
                var currentPoint = processList[index];
                bool found = false;

                var minimalDistance = SampleMinimalDistance(minimalDistanceField, currentPoint);

                for (int i = 0; i < _maxAttempts; i++)
                {
                    var candidate = _random.SelectRandomAroundPoint(currentPoint, minimalDistance);
                    if (!grid.Contains(candidate))
                        continue;

                    var candidateMinimalDistance = SampleMinimalDistance(minimalDistanceField, candidate);

                    if (grid.TryAdd(candidate, candidateMinimalDistance))
                    {
                        samples.Add(new PoissonDiskPointSample(candidate, candidateMinimalDistance));
                        processList.Add(candidate);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    processList.RemoveAt(index);
            }

            return samples;
        }

        private static float SampleMinimalDistance(IFloatField minimalDistanceField, VectorXY point)
        {
            float minimalDistance = minimalDistanceField.Sample(point);
            if (minimalDistance <= 0f)
                throw new InvalidOperationException("Minimal distance field returned a non-positive value.");

            return minimalDistance;
        }
    }
}
