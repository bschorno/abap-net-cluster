namespace ABAPNet.Cluster
{
    internal interface IDataBufferContext
    {
        DataBufferConfiguration Configuration { get; }

        DataBufferSegment? CurrentSegment { get; }
    }
}
