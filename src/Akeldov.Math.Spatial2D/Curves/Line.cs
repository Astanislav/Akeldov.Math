using Akeldov.Math.Spatial2D;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    /// <summary>
    /// Represents an infinite two-dimensional line defined by two points.
    /// </summary>
    public readonly struct Line : IProjectableCurve, IEquatable<Line>
    {
        private readonly float _equationA;
        private readonly float _equationB;
        private readonly float _equationC;
        private readonly VectorXY _direction;
        private readonly VectorXY _origin;

        /// <summary>
        /// Initializes a new line passing through the specified points.
        /// </summary>
        /// <param name="a">The first point defining the line.</param>
        /// <param name="b">The second point defining the line.</param>
        /// <exception cref="ArgumentException">Thrown when the points are equal.</exception>
        public Line(VectorXY a, VectorXY b)
            : this(a, b, LineReferencePointMode.GlobalZero)
        {
        }

        /// <summary>
        /// Initializes a new line passing through the specified points and selects the parameter origin
        /// from the specified reference point mode.
        /// </summary>
        /// <param name="a">The first point defining the line.</param>
        /// <param name="b">The second point defining the line.</param>
        /// <param name="referencePointMode">The mode used to select the reference point.</param>
        /// <exception cref="ArgumentException">Thrown when the points are equal.</exception>
        public Line(VectorXY a, VectorXY b, LineReferencePointMode referencePointMode)
            : this(a, b, SelectReferencePoint(a, b, referencePointMode))
        {
        }

        /// <summary>
        /// Initializes a new line passing through the specified points and uses the projection of
        /// the reference point as the parameter origin.
        /// </summary>
        /// <param name="a">The first point defining the line.</param>
        /// <param name="b">The second point defining the line.</param>
        /// <param name="referencePoint">The point whose projection becomes the parameter origin.</param>
        /// <exception cref="ArgumentException">Thrown when the points are equal.</exception>
        public Line(VectorXY a, VectorXY b, VectorXY referencePoint)
        {
            if (a.Equals(b))
                throw new ArgumentException("Line endpoints must be distinct.", nameof(b));

            float equationA = b.Y - a.Y;
            float equationB = a.X - b.X;
            float equationC = -(equationA * a.X + equationB * a.Y);

            Initialize(equationA, equationB, equationC, referencePoint,
                out _equationA,
                out _equationB,
                out _equationC,
                out _direction,
                out _origin);
        }

        /// <summary>
        /// Initializes a new line from the implicit equation <c>ax + by + c = 0</c>.
        /// </summary>
        /// <param name="a">The X coefficient.</param>
        /// <param name="b">The Y coefficient.</param>
        /// <param name="c">The offset coefficient.</param>
        /// <exception cref="ArgumentException">Thrown when both linear coefficients are zero.</exception>
        public Line(float a, float b, float c)
            : this(a, b, c, VectorXY.Zero)
        {
        }

        /// <summary>
        /// Initializes a new line from the implicit equation <c>ax + by + c = 0</c> and uses the
        /// projection of the reference point as the parameter origin.
        /// </summary>
        /// <param name="a">The X coefficient.</param>
        /// <param name="b">The Y coefficient.</param>
        /// <param name="c">The offset coefficient.</param>
        /// <param name="referencePoint">The point whose projection becomes the parameter origin.</param>
        /// <exception cref="ArgumentException">Thrown when both linear coefficients are zero.</exception>
        public Line(float a, float b, float c, VectorXY referencePoint)
        {
            Initialize(a, b, c, referencePoint,
                out _equationA,
                out _equationB,
                out _equationC,
                out _direction,
                out _origin);
        }

        /// <summary>
        /// Gets a representative point on the line.
        /// </summary>
        public VectorXY A => _origin;

        /// <summary>
        /// Gets a second representative point on the line.
        /// </summary>
        public VectorXY B => _origin + _direction;

        /// <summary>
        /// Gets the normalized X coefficient of the implicit equation <c>ax + by + c = 0</c>.
        /// </summary>
        public float EquationA => _equationA;

        /// <summary>
        /// Gets the normalized Y coefficient of the implicit equation <c>ax + by + c = 0</c>.
        /// </summary>
        public float EquationB => _equationB;

        /// <summary>
        /// Gets the normalized offset coefficient of the implicit equation <c>ax + by + c = 0</c>.
        /// </summary>
        public float EquationC => _equationC;

        /// <summary>
        /// Gets the normalized vector perpendicular to this line.
        /// </summary>
        public VectorXY Normal => new VectorXY(_equationA, _equationB);

        /// <summary>
        /// Gets the normalized direction vector of this line.
        /// </summary>
        public VectorXY Direction => _direction;

        /// <summary>
        /// Gets the point on this line from which projection parameters are measured.
        /// </summary>
        public VectorXY Origin => _origin;

        /// <summary>
        /// Gets the closest point on this line to the global coordinate origin.
        /// </summary>
        public VectorXY ClosestPointToOrigin => -_equationC * Normal;

        /// <summary>
        /// Returns the shortest distance from the specified point to this line.
        /// </summary>
        /// <param name="point">The point to measure from.</param>
        /// <returns>The distance to this line.</returns>
        public float Distance(VectorXY point)
        {
            return MathF.Abs(GetSignedDistance(point));
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Line other && Equals(other);

        /// <summary>
        /// Indicates whether this line has the same normalized implicit equation as another line.
        /// </summary>
        /// <param name="other">The line to compare with this line.</param>
        /// <returns><see langword="true"/> if both lines are equal; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Line other) =>
            EquationA.Equals(other.EquationA) &&
            EquationB.Equals(other.EquationB) &&
            EquationC.Equals(other.EquationC);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(EquationA, EquationB, EquationC);

        /// <summary>
        /// Returns point intersections with the specified ray. If the ray lies on this line,
        /// returns the ray origin as the first point encountered by the ray.
        /// </summary>
        /// <param name="ray">The ray to intersect with this line.</param>
        /// <returns>The intersection points in the forward direction of the ray.</returns>
        public List<VectorXY> RayIntersections(Ray ray)
        {
            List<VectorXY> intersections = new List<VectorXY>();

            VectorXY p = ray.Origin;
            VectorXY r = ray.Dir;
            VectorXY q = Origin;
            VectorXY s = Direction;

            float cross = VectorXY.Cross(r, s);

            if (cross.IsAlmostZero())
            {
                if (s.SQRLength <= GeometryConstants.GeometryEpsilonSquared)
                {
                    if (IsPointOnRay(q, ray))
                        intersections.AddDistinct(q);
                }
                else if (VectorXY.Cross(q - p, s).IsAlmostZero())
                {
                    intersections.AddDistinct(p);
                }

                return intersections;
            }

            VectorXY qMinusP = q - p;
            float t = VectorXY.Cross(qMinusP, s) / cross;

            if (t >= 0)
            {
                VectorXY intersection = p + r * t;
                intersections.AddDistinct(intersection);
            }

            return intersections;
        }

        private static bool IsPointOnRay(VectorXY point, Ray ray)
        {
            VectorXY toPoint = point - ray.Origin;

            if (VectorXY.Dot(toPoint, ray.Dir) < -GeometryConstants.GeometryEpsilon)
                return false;

            return VectorXY.Cross(toPoint, ray.Dir).IsAlmostZero();
        }

        /// <summary>
        /// Projects the specified point onto this line and returns only the projected point.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The closest point on this line.</returns>
        public VectorXY ProjectPoint(VectorXY point)
        {
            return Project(point).Point;
        }

        /// <summary>
        /// Projects the specified point onto this line.
        /// </summary>
        /// <param name="point">The point to project.</param>
        /// <returns>The projection point, line parameter, and distance to this line.</returns>
        public CurveProjection Project(VectorXY point)
        {
            float signedDistance = GetSignedDistance(point);
            VectorXY projection = point - Normal * signedDistance;
            float parameter = VectorXY.Dot(projection - Origin, Direction);

            return new CurveProjection(projection, parameter, MathF.Abs(signedDistance));
        }

        /// <inheritdoc/>
        public override string ToString() => $"({A} - {B})";

        /// <summary>
        /// Indicates whether two lines are equal.
        /// </summary>
        /// <param name="left">The first line.</param>
        /// <param name="right">The second line.</param>
        /// <returns><see langword="true"/> if the lines are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Line left, Line right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two lines are different.
        /// </summary>
        /// <param name="left">The first line.</param>
        /// <param name="right">The second line.</param>
        /// <returns><see langword="true"/> if the lines are different; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Line left, Line right) => !(left == right);

        private float GetSignedDistance(VectorXY point)
        {
            return _equationA * point.X + _equationB * point.Y + _equationC;
        }

        private static void Initialize(
            float equationA,
            float equationB,
            float equationC,
            VectorXY referencePoint,
            out float normalizedA,
            out float normalizedB,
            out float normalizedC,
            out VectorXY direction,
            out VectorXY origin)
        {
            float scale = MathF.Sqrt(equationA * equationA + equationB * equationB);
            if (scale == 0f)
                throw new ArgumentException("Line equation must have at least one non-zero linear coefficient.");

            normalizedA = equationA / scale;
            normalizedB = equationB / scale;
            normalizedC = equationC / scale;

            if (normalizedA < 0f || (normalizedA == 0f && normalizedB < 0f))
            {
                normalizedA = -normalizedA;
                normalizedB = -normalizedB;
                normalizedC = -normalizedC;
            }

            var normal = new VectorXY(normalizedA, normalizedB);
            direction = new VectorXY(normalizedB, -normalizedA);

            float signedDistance = normalizedA * referencePoint.X + normalizedB * referencePoint.Y + normalizedC;
            origin = referencePoint - normal * signedDistance;
        }

        private static VectorXY SelectReferencePoint(VectorXY a, VectorXY b, LineReferencePointMode referencePointMode)
        {
            switch (referencePointMode)
            {
                case LineReferencePointMode.PointA:
                    return a;
                case LineReferencePointMode.PointB:
                    return b;
                case LineReferencePointMode.Middle:
                    return (a + b) * 0.5f;
                default:
                    return VectorXY.Zero;
            }
        }
    }
}
