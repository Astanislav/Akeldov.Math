using Akeldov.Math.Vectors.XY;

namespace Akeldov.Math.Geometry.Curves
{
    public interface ICurve
    {
        List<VectorXY> RayIntersections(Ray ray);

        float Distance(VectorXY point);
    }
}
