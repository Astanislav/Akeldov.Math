namespace Akeldov.Math.Sampling
{
    public interface ISampler<in TSamplingContext, out TSamplingResult, out TSample>
        where TSamplingResult : ISamplingResult<TSample>
    {
        TSamplingResult Sample(TSamplingContext samplingContext);
    }
}