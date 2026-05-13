using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Fields
{
    /// <summary>
    /// Selects the Delaunay triangle containing the sampled point.
    /// </summary>
    /// <remarks>
    /// When the sampled point is outside the triangulated area, this culler returns the nearest
    /// convex hull feature: a single hull vertex or the two endpoints of a hull edge. When the
    /// source points cannot form a non-degenerate hull, it falls back to half-plane culling and
    /// keeps at most two sources.
    /// </remarks>
    /// <typeparam name="TPointSource">The point influence source type.</typeparam>
    public sealed class DelaunayCuller<TPointSource> : IInfluenceSourceCuller<TPointSource>
        where TPointSource : IPointInfluenceSource
    {
        private const float Eps = GeometryConstants.GeometryEpsilon;

        private readonly TPointSource[] _sources;
        private readonly VectorXY[] _points;
        private readonly Triangle[] _triangles;
        private readonly int[] _hull;
        private readonly HalfPlaneCuller<TPointSource> _fallbackCuller;

        /// <summary>
        /// Initializes a new Delaunay influence source culler.
        /// </summary>
        /// <param name="pointSources">The point influence sources used to build the triangulation.</param>
        public DelaunayCuller(IReadOnlyList<TPointSource> pointSources)
        {
            if (pointSources == null)
                throw new ArgumentNullException(nameof(pointSources));

            int n = pointSources.Count;
            if (n < 3)
                throw new ArgumentException("At least three influence points are required.", nameof(pointSources));

            _sources = new TPointSource[n];
            _points = new VectorXY[n];

            for (int i = 0; i < n; i++)
            {
                var pointSource = pointSources[i];
                if (pointSource is null)
                    throw new ArgumentException("Influence source collection cannot contain null elements.", nameof(pointSources));

                _sources[i] = pointSource;
                _points[i] = pointSource.Position;
            }

            ThrowIfAnyDuplicatePoint(_points, nameof(pointSources));

            _fallbackCuller = new HalfPlaneCuller<TPointSource>(_sources);

            if (HasNonCollinearTriple(_points))
            {
                _hull = BuildConvexHull(_points);
                _triangles = BuildDelaunay(_points);
            }
            else
            {
                _hull = Array.Empty<int>();
                _triangles = Array.Empty<Triangle>();
            }
        }

        /// <summary>
        /// Returns influence sources selected for the specified point.
        /// </summary>
        /// <param name="point">The point being sampled.</param>
        /// <returns>
        /// A new mutable list owned by the caller containing the selected triangle sources,
        /// nearest hull feature sources, or fallback sources.
        /// </returns>
        public List<TPointSource> Cull(VectorXY point)
        {
            var res = new List<TPointSource>(3);

            for (int i = 0; i < _triangles.Length; i++)
            {
                if (PointInTriangle(point, _triangles[i]))
                {
                    AddTriangle(res, _triangles[i]);
                    return res;
                }
            }

            if (_hull.Length >= 3)
                return CullHullBoundary(point);

            return FilterFallbackSources(_fallbackCuller.Cull(point), point);
        }

        private void AddTriangle(List<TPointSource> res, Triangle t)
        {
            res.Add(_sources[t.A]);
            res.Add(_sources[t.B]);
            res.Add(_sources[t.C]);
        }

        private static List<TPointSource> FilterFallbackSources(List<TPointSource> sources, VectorXY point)
        {
            if (sources.Count <= 2)
                return sources;

            int bestA = 0;
            int bestB = 1;
            float bestDistanceSquared = DistanceToSegmentSquared(sources[0].Position, sources[1].Position, point);

            for (int i = 0; i < sources.Count; i++)
            {
                for (int j = i + 1; j < sources.Count; j++)
                {
                    float distanceSquared = DistanceToSegmentSquared(sources[i].Position, sources[j].Position, point);
                    if (distanceSquared < bestDistanceSquared)
                    {
                        bestA = i;
                        bestB = j;
                        bestDistanceSquared = distanceSquared;
                    }
                }
            }

            return new List<TPointSource>(2)
            {
                sources[bestA],
                sources[bestB]
            };
        }

        private List<TPointSource> CullHullBoundary(VectorXY point)
        {
            int bestStart = _hull[0];
            int bestEnd = _hull[1];
            float bestParameter = 0f;
            float bestDistanceSquared = DistanceToSegmentSquared(
                _points[bestStart],
                _points[bestEnd],
                point,
                out bestParameter);

            for (int i = 1; i < _hull.Length; i++)
            {
                int start = _hull[i];
                int end = _hull[(i + 1) % _hull.Length];
                float parameter;
                float distanceSquared = DistanceToSegmentSquared(
                    _points[start],
                    _points[end],
                    point,
                    out parameter);

                if (distanceSquared < bestDistanceSquared)
                {
                    bestStart = start;
                    bestEnd = end;
                    bestParameter = parameter;
                    bestDistanceSquared = distanceSquared;
                }
            }

            if (bestParameter <= Eps)
                return new List<TPointSource>(1) { _sources[bestStart] };

            if (bestParameter >= 1f - Eps)
                return new List<TPointSource>(1) { _sources[bestEnd] };

            return new List<TPointSource>(2)
            {
                _sources[bestStart],
                _sources[bestEnd]
            };
        }

        private static Triangle[] BuildDelaunay(VectorXY[] pts)
        {
            int n = pts.Length;

            var all = new VectorXY[n + 3];
            Array.Copy(pts, all, n);

            var (sa, sb, sc) = SuperTriangle(pts);
            all[n] = sa; all[n + 1] = sb; all[n + 2] = sc;

            var triangles = new List<Triangle>();
            if (Triangle.TryCreate(n, n + 1, n + 2, all, out var superTriangle))
                triangles.Add(superTriangle);

            if (triangles.Count == 0)
                return Array.Empty<Triangle>();

            var keptTriangles = new List<Triangle>();
            var edgeCounts = new Dictionary<Edge, int>();

            for (int i = 0; i < n; i++)
            {
                keptTriangles.Clear();
                edgeCounts.Clear();

                for (int j = 0; j < triangles.Count; j++)
                {
                    var t = triangles[j];
                    if (t.InCircumcircle(all[i]))
                    {
                        AddEdge(edgeCounts, t.A, t.B);
                        AddEdge(edgeCounts, t.B, t.C);
                        AddEdge(edgeCounts, t.C, t.A);
                    }
                    else
                    {
                        keptTriangles.Add(t);
                    }
                }

                var oldTriangles = triangles;
                triangles = keptTriangles;
                keptTriangles = oldTriangles;

                foreach (var edgeCount in edgeCounts)
                    if (edgeCount.Value == 1 &&
                        Triangle.TryCreate(edgeCount.Key.U, edgeCount.Key.V, i, all, out var triangle))
                        triangles.Add(triangle);
            }

            triangles.RemoveAll(t => t.A >= n || t.B >= n || t.C >= n);
            return triangles.ToArray();
        }

        private static void AddEdge(Dictionary<Edge, int> edgeCounts, int u, int v)
        {
            var edge = new Edge(u, v);
            if (edgeCounts.TryGetValue(edge, out int count))
                edgeCounts[edge] = count + 1;
            else
                edgeCounts.Add(edge, 1);
        }


        private readonly struct Triangle
        {
            public readonly int A, B, C;
            private readonly VectorXY _center;
            private readonly float _r2;

            private Triangle(int a, int b, int c, VectorXY center, float r2)
            {
                A = a; B = b; C = c;
                _center = center;
                _r2 = r2;
            }

            public static bool TryCreate(int a, int b, int c, VectorXY[] pts, out Triangle triangle)
            {
                if (!TryCircumcircle(pts[a], pts[b], pts[c], out var center, out float r2))
                {
                    triangle = default;
                    return false;
                }

                triangle = new Triangle(a, b, c, center, r2);
                return true;
            }

            public bool InCircumcircle(VectorXY p)
                => Dist2(p, _center) <= _r2 + GeometryConstants.GeometryEpsilon;
        }

        private readonly struct Edge : IEquatable<Edge>
        {
            public readonly int U, V;
            public Edge(int u, int v)
            {
                if (u < v) { U = u; V = v; }
                else { U = v; V = u; }
            }
            public bool Equals(Edge other) => U == other.U && V == other.V;

            public override bool Equals(object? obj) => obj is Edge other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(U, V);
        }

        private bool PointInTriangle(VectorXY p, Triangle t)
        {
            VectorXY a = _points[t.A];
            VectorXY b = _points[t.B];
            VectorXY c = _points[t.C];

            float s1 = Cross(b - a, p - a);
            float s2 = Cross(c - b, p - b);
            float s3 = Cross(a - c, p - c);

            bool neg = (s1 < -Eps) || (s2 < -Eps) || (s3 < -Eps);
            bool pos = (s1 > Eps) || (s2 > Eps) || (s3 > Eps);

            return !(neg && pos);
        }

        private static float Cross(VectorXY u, VectorXY v)
            => u.X * v.Y - u.Y * v.X;

        private static float Dist2(VectorXY a, VectorXY b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        private static float DistanceToSegmentSquared(VectorXY a, VectorXY b, VectorXY point)
        {
            return DistanceToSegmentSquared(a, b, point, out _);
        }

        private static float DistanceToSegmentSquared(VectorXY a, VectorXY b, VectorXY point, out float parameter)
        {
            VectorXY ab = b - a;
            float ab2 = ab.SquaredLength;
            if (ab2 <= GeometryConstants.GeometryEpsilonSquared)
            {
                parameter = 0f;
                return Dist2(a, point);
            }

            parameter = VectorXY.Dot(point - a, ab) / ab2;
            if (parameter < 0f)
                parameter = 0f;
            else if (parameter > 1f)
                parameter = 1f;

            VectorXY projection = a + parameter * ab;
            return Dist2(projection, point);
        }

        private static int[] BuildConvexHull(VectorXY[] points)
        {
            int start = 0;
            for (int i = 1; i < points.Length; i++)
            {
                VectorXY candidate = points[i];
                VectorXY current = points[start];
                if (candidate.X < current.X || (candidate.X == current.X && candidate.Y < current.Y))
                    start = i;
            }

            var hull = new List<int>();
            int currentIndex = start;

            do
            {
                hull.Add(currentIndex);
                int nextIndex = currentIndex == 0 ? 1 : 0;

                for (int i = 0; i < points.Length; i++)
                {
                    if (i == currentIndex)
                        continue;

                    VectorXY current = points[currentIndex];
                    VectorXY next = points[nextIndex];
                    VectorXY candidate = points[i];
                    float cross = Cross(next - current, candidate - current);

                    if (cross < -Eps ||
                        (cross.IsAlmostZero() && Dist2(current, candidate) > Dist2(current, next)))
                    {
                        nextIndex = i;
                    }
                }

                currentIndex = nextIndex;
            }
            while (currentIndex != start && hull.Count <= points.Length);

            return hull.ToArray();
        }

        private static (VectorXY, VectorXY, VectorXY)
            SuperTriangle(VectorXY[] pts)
        {
            float minX = pts[0].X, minY = pts[0].Y;
            float maxX = minX, maxY = minY;

            for (int i = 1; i < pts.Length; i++)
            {
                var p = pts[i];
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.X > maxX) maxX = p.X;
                if (p.Y > maxY) maxY = p.Y;
            }

            float d = MathF.Max(maxX - minX, maxY - minY);
            float cx = (minX + maxX) * 0.5f;
            float cy = (minY + maxY) * 0.5f;

            return (
                new VectorXY(cx - 20 * d, cy - 10 * d),
                new VectorXY(cx, cy + 20 * d),
                new VectorXY(cx + 20 * d, cy - 10 * d)
            );
        }

        private static void ThrowIfAnyDuplicatePoint(VectorXY[] points, string paramName)
        {
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    if (points[i].AlmostEquals(points[j]))
                        throw new ArgumentException("Influence points must have distinct centers.", paramName);
                }
            }
        }

        private static bool HasNonCollinearTriple(VectorXY[] points)
        {
            VectorXY a = points[0];
            VectorXY b = points[1];

            for (int i = 2; i < points.Length; i++)
            {
                if (!Cross(b - a, points[i] - a).IsAlmostZero())
                    return true;
            }

            return false;
        }

        private static bool TryCircumcircle(VectorXY a, VectorXY b, VectorXY c, out VectorXY center, out float r2)
        {
            float ax = a.X, ay = a.Y;
            float bx = b.X, by = b.Y;
            float cx = c.X, cy = c.Y;

            float d = 2f * (ax * (by - cy) +
                            bx * (cy - ay) +
                            cx * (ay - by));

            if (d.IsAlmostZero())
            {
                center = default;
                r2 = 0f;
                return false;
            }

            float ax2 = ax * ax + ay * ay;
            float bx2 = bx * bx + by * by;
            float cx2 = cx * cx + cy * cy;

            float ux = (ax2 * (by - cy) +
                        bx2 * (cy - ay) +
                        cx2 * (ay - by)) / d;

            float uy = (ax2 * (cx - bx) +
                        bx2 * (ax - cx) +
                        cx2 * (bx - ax)) / d;

            center = new VectorXY(ux, uy);
            r2 = Dist2(center, a);
            return true;
        }
    }

}
