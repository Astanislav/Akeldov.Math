namespace Akeldov.Math.Spatial2D.Curves
{
    public interface IProjectableCurve : ICurve
    {
        CurveProjection Project(VectorXY point);
    }
}