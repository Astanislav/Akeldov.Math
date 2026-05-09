using System.Collections.Generic;

namespace Akeldov.Math.Spatial2D.Partitioning
{
    public interface IPartition<TItem>
    {
        IReadOnlyList<TItem> Items { get; }
    }
}
