namespace Akeldov.Math.Spatial2D.Fields
{
    public interface IWeightedAdditive<T>
    {
        T Add(T other);

        T Multiply(float weight);
    }
}
