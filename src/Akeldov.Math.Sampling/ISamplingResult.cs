namespace Akeldov.Math.Sampling
{
    public interface ISamplingResult<out TSample>
    {
        IReadOnlyList<TSample> Samples { get; }
    }
}