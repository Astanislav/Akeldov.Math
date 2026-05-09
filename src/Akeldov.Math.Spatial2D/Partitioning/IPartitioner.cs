using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning
{
    public interface IPartitioner<TPartition, TItem>
        where TPartition : IPartition<TItem>
    {
        IReadOnlyList<TPartition> Partition(IReadOnlyList<TItem> items);
    }
}
