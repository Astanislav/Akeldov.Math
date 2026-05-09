namespace Akeldov.Math.Spatial2D
{
    public interface IScalable<out TSelf>
    {
        TSelf Scale(VectorXY scale);
    }
}
