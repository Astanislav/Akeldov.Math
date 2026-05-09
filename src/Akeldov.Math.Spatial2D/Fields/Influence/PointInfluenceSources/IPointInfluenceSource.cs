namespace Akeldov.Math.Spatial2D.Fields
{
    public interface IPointInfluenceSource : IInfluenceSource, IHasPosition2D
    {
        float Power { get; }
    }

    public interface IPointInfluenceSource<TValue> : IPointInfluenceSource, IInfluenceSource<TValue>
    {
        TValue Value { get; }
    }
}
