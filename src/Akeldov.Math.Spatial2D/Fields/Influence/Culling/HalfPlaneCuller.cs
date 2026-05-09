using Akeldov.Math.Spatial2D.Curves;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    public class HalfPlaneCuller<TPointSource> : IInfluenceSourceCuller<TPointSource>
        where TPointSource : IPointInfluenceSource
    {
        private readonly IReadOnlyList<TPointSource> _sourcePoints;

        public HalfPlaneCuller(IReadOnlyList<TPointSource> sourcePoints)
        {
            if (sourcePoints == null)
                throw new ArgumentNullException(nameof(sourcePoints));

            if (sourcePoints.Count == 0)
                throw new ArgumentException("Influence source collection must not be empty.", nameof(sourcePoints));

            _sourcePoints = sourcePoints;
        }

        public List<TPointSource> Cull(VectorXY point)
        {
            var orderedSourcePoints = OrderBy(_sourcePoints, x => x.Center.Distance(point));
            var culledSourcePoints = new List<TPointSource>();
            var lines = new List<Line>();
            for (int i = 0; i < orderedSourcePoints.Count; i++)
            {
                var sourcePoint = orderedSourcePoints[i];
                bool isExcluded = false;

                for (int j = 0; j < lines.Count; j++)
                {
                    var line = lines[j];
                    if (!line.IsSameSide(point, sourcePoint.Center))
                    {
                        isExcluded = true;
                        break;
                    }
                }

                if (!isExcluded)
                {
                    culledSourcePoints.Add(sourcePoint);

                    if (sourcePoint.Center.Distance(point) <= GeometryConstants.GeometryEpsilon)
                        continue;

                    var line = new Line(point, sourcePoint.Center);
                    var perpendicular = line.PerpendicularAt(sourcePoint.Center);
                    lines.Add(perpendicular);
                }
            }

            return culledSourcePoints;
        }

        private List<TEntity> OrderBy<TEntity, TProperty>(
            IReadOnlyList<TEntity> source, Func<TEntity, TProperty> selector)
            where TProperty : IComparable<TProperty>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = new List<TEntity>(source);

            result.Sort((x, y) =>
            {
                return (selector(x)).CompareTo(selector(y));
            });

            return result;
        }
    }
}
