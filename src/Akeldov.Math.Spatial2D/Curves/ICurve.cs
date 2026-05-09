using Akeldov.Math.Spatial2D;
using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Curves
{
    public interface ICurve
    {
        List<VectorXY> RayIntersections(Ray ray);

        float Distance(VectorXY point);
    }
}
