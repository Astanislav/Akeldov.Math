namespace Akeldov.Math.Hexes.Topology
{
    public interface IPolyhexWithPriority : IPolyhex
    {
        int Priority { get; }
    }
}
