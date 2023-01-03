namespace ABAPNet.Cluster.Converter.Types
{
    internal interface IDefinedType : IType
    {
        int StructDescrByteLength { get; }
    }
}
