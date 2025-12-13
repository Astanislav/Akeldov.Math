using Akeldov.Math.Vectors.XY;
using System.Collections.Generic;

namespace Akeldov.Math.Geometry.Curves
{
    public interface ICurve
    {
        List<VectorXY> RayIntersections(Ray ray);

        float Distance(VectorXY point);
    }
}
