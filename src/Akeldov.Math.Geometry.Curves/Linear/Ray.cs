using Akeldov.Math.Vectors.XY;
using System;
using System.Collections.Generic;

namespace Akeldov.Math.Geometry.Curves
{
    public struct Ray : ICurve
    {
        private VectorXY _origin;
        private float _angleRad;
        private VectorXY _dir;

        public Ray(VectorXY origin)
        {
            _origin = origin;
            _angleRad = 0;
            _dir = new VectorXY(1, 0);
        }

        public Ray(VectorXY origin, float angleRad)
        {
            _origin = origin;
            _angleRad = angleRad;
            _dir = new VectorXY(MathF.Cos(angleRad), MathF.Sin(angleRad));
        }

        public VectorXY Origin => _origin;

        public VectorXY Dir => _dir;

        public float AngleRad => _angleRad;

        public float Distance(VectorXY point)
        {
            VectorXY toPoint = point - _origin;

            float dot = VectorXY.Dot(toPoint, Dir);

            if (dot < 0)
            {
                return (point - _origin).Length;
            }
            else
            {
                VectorXY projected = _origin + Dir * dot;
                return (point - projected).Length;
            }
        }

        public List<VectorXY> RayIntersections(Ray other)
        {
            List<VectorXY> intersections = new List<VectorXY>();

            VectorXY p = _origin;
            VectorXY r = Dir;

            VectorXY q = other._origin;
            VectorXY s = other.Dir;

            float cross = VectorXY.Cross(r, s);

            if (MathF.Abs(cross) < 1e-6f)
            {
                return intersections;
            }

            VectorXY qMinusP = q - p;

            float t = VectorXY.Cross(qMinusP, s) / cross;
            float u = VectorXY.Cross(qMinusP, r) / cross;

            if (t >= 0 && u >= 0)
            {
                VectorXY intersectionPoint = p + r * t;
                intersections.Add(intersectionPoint);
            }

            return intersections;
        }
    }
}
