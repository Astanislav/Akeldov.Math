using Akeldov.Math.Vectors.XY;

namespace Akeldov.Math.Sampling
{
    public interface IPointSampler<in TSamplingContext, out TSamplingResult> : ISampler<TSamplingContext, TSamplingResult, VectorXY>
        where TSamplingResult : ISamplingResult<VectorXY>
    {
    }
}