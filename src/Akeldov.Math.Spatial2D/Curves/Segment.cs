using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents a finite line segment in two-dimensional space.
    /// </summary>
    public readonly struct Segment : IProjectableCurve, IEquatable<Segment>
    {
        private readonly VectorXY _a;
        private readonly VectorXY _b;

        private readonly bool _includesA;
        private readonly bool _includesB;

        /// <summary>
        /// Initializes a new segment with both endpoints included.
        /// </summary>
        /// <param name="a">The first endpoint.</param>
        /// <param name="b">The second endpoint.</param>
        public Segment(VectorXY a, VectorXY b)
        {
            _a = a;
            _b = b;
            _includesA = true;
            _includesB = true;
        }

        /// <summary>
        /// Initializes a new segment with explicit endpoint inclusion.
        /// </summary>
        /// <param name="a">The first endpoint.</param>
        /// <param name="b">The second endpoint.</param>
        /// <param name="includesA">Whether the first endpoint belongs to the segment.</param>
        /// <param name="includesB">Whether the second endpoint belongs to the segment.</param>
        public Segment(VectorXY a, VectorXY b, bool includesA, bool includesB)
        {
            _a = a;
            _b = b;
            _includesA = includesA;
            _includesB = includesB;
        }

        /// <summary>
        /// Gets the first endpoint.
        /// </summary>
        public VectorXY A => _a;

        /// <summary>
        /// Gets the second endpoint.
        /// </summary>
        public VectorXY B => _b;

        /// <summary>
        /// Gets a value indicating whether the first endpoint belongs to the segment.
        /// </summary>
        public bool IncludesA => _includesA;

        /// <summary>
        /// Gets a value indicating whether the second endpoint belongs to the segment.
        /// </summary>
        public bool IncludesB => _includesB;

        /// <summary>
        /// Returns point intersections with the specified ray. If the ray is collinear with the segment
        /// and their overlap is not a single point, returns the first included point encountered along the ray.
        /// If the overlap starts at an excluded endpoint, no first point exists and the result is empty.
        /// </summary>
        /// <param name="ray">The ray to intersect with this segment.</param>
        /// <returns>The intersection points in the forward direction of the ray.</returns>
        public List<VectorXY> RayIntersections(Ray ray)
        {
            List<VectorXY> intersections = new List<VectorXY>();

            VectorXY rayDir = ray.Dir;
            VectorXY segDir = B - A;
            VectorXY delta = A - ray.Origin;

            float det = rayDir.X * (-segDir.Y) + rayDir.Y * segDir.X;

            if (det.IsAlmostZero())
            {
                AddFirstCollinearIntersection(ray, intersections);
                return intersections;
            }

            float t = (delta.X * -segDir.Y + delta.Y * segDir.X) / det;
            float u = (rayDir.X * delta.Y - rayDir.Y * delta.X) / det;

            if (t >= 0f && u >= 0f && u <= 1f)
            {
                VectorXY intersection = ray.Origin + t * rayDir;

                if (intersection.AlmostEquals(A))
                {
                    if (IncludesA)
                        intersections.AddDistinct(intersection);
                }
                else if (intersection.AlmostEquals(B))
                {
                    if (IncludesB)
                        intersections.AddDistinct(intersection);
                }
                else
                {
                    intersections.AddDistinct(intersection);
                }
            }

            return intersections;
        }

        private void AddFirstCollinearIntersection(Ray ray, List<VectorXY> intersections)
        {
            VectorXY segDir = B - A;
            VectorXY originToA = A - ray.Origin;

            if (!VectorXY.Cross(originToA, ray.Dir).IsAlmostZero())
                return;

            if (segDir.SQRLength <= GeometryConstants.GeometryEpsilonSquared)
            {
                if ((IncludesA || IncludesB) && IsPointOnRay(A, ray, out _))
                    intersections.AddDistinct(A);

                return;
            }

            float tA = VectorXY.Dot(A - ray.Origin, ray.Dir);
            float tB = VectorXY.Dot(B - ray.Origin, ray.Dir);

            VectorXY startPoint;
            bool startIncluded;
            float startT;
            float endT;

            if (tA <= tB)
            {
                startPoint = A;
                startIncluded = IncludesA;
                startT = tA;
                endT = tB;
            }
            else
            {
                startPoint = B;
                startIncluded = IncludesB;
                startT = tB;
                endT = tA;
            }

            if (endT < -GeometryConstants.GeometryEpsilon)
                return;

            float firstT = MathF.Max(0f, startT);
            if (firstT > endT + GeometryConstants.GeometryEpsilon)
                return;

            if (firstT.IsAlmostZero())
            {
                if (IncludesPoint(ray.Origin))
                    intersections.AddDistinct(ray.Origin);

                return;
            }

            if (startIncluded)
                intersections.AddDistinct(startPoint);
        }

        private bool IncludesPoint(VectorXY point)
        {
            VectorXY ab = B - A;
            VectorXY ap = point - A;

            if (!VectorXY.Cross(ab, ap).IsAlmostZero())
                return false;

            float dot = VectorXY.Dot(ap, ab);
            if (dot < -GeometryConstants.GeometryEpsilon)
                return false;

            if (dot > ab.SQRLength + GeometryConstants.GeometryEpsilon)
                return false;

            if (point.AlmostEquals(A) && !IncludesA)
                return false;

            if (point.AlmostEquals(B) && !IncludesB)
                return false;

            return true;
        }

        private static bool IsPointOnRay(VectorXY point, Ray ray, out float t)
        {
            VectorXY toPoint = point - ray.Origin;
            t = VectorXY.Dot(toPoint, ray.Dir);

            if (t < -GeometryConstants.GeometryEpsilon)
                return false;

            return VectorXY.Cross(toPoint, ray.Dir).IsAlmostZero();
        }

        public override bool Equals(object? obj) => obj is Segment other && Equals(other);

        public bool Equals(Segment other) =>
            A.Equals(other.A) &&
            B.Equals(other.B) &&
            IncludesA == other.IncludesA &&
            IncludesB == other.IncludesB;

        public override int GetHashCode() => HashCode.Combine(A, B, IncludesA, IncludesB);

        public override string ToString() => $"({A} - {B})";

        /// <summary>
        /// Returns the shortest distance from the specified point to this segment.
        /// </summary>
        /// <param name="point">The point to measure from.</param>
        /// <returns>The distance to this segment.</returns>
        public float Distance(VectorXY point)
        {
            return Project(point).Distance;
        }

        /// <summary>
        /// Projects the specified point onto this segment.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point, segment parameter, and distance to this segment.</returns>
        public CurveProjection Project(VectorXY point)
        {
            VectorXY ab = B - A;
            VectorXY ap = point - A;

            float abSquared = ab.SQRLength;
            if (abSquared <= GeometryConstants.GeometryEpsilonSquared)
                return new CurveProjection(A, 0f, point.Distance(A));

            float t = VectorXY.Dot(ap, ab) / abSquared;

            if (t < 0f)
                t = 0f;
            else if (t > 1f)
                t = 1f;

            VectorXY projection = A + t * ab;
            return new CurveProjection(projection, t, point.Distance(projection));
        }

        public static bool operator ==(Segment left, Segment right) => left.Equals(right);

        public static bool operator !=(Segment left, Segment right) => !(left == right);

        public static Segment operator +(Segment left, VectorXY right) => new Segment(left.A + right, left.B + right, left.IncludesA, left.IncludesB);

        public static Segment operator -(Segment left, VectorXY right) => new Segment(left.A - right, left.B - right, left.IncludesA, left.IncludesB);
    }
}
