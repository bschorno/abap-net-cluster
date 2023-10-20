using ABAPNet.Cluster;
using ABAPNet.Cluster.Attributes;

DataBuffer dataBuffer = new DataBuffer();
byte[] byteData = dataBuffer.Export(new Cluster());

internal sealed class Cluster
{
    [ClusterFieldName("DATA")]
    [DeepTable]
    public ClusterStruct[]? Data { get; set; }
}

internal sealed class ClusterStruct
{
    [Int4]
    public int Id { get; set; }

    [DeepStruct]
    public ClusterValue? Value { get; set; }
}

internal struct ClusterValue
{
    [String]
    public string? Type { get; set; }

    [String]
    public string? Value { get; set; }

    public ClusterValue(string? type, string? value)
    {
        Type = type;
        Value = value;
    }
}