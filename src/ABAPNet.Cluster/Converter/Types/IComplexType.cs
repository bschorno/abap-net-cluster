namespace ABAPNet.Cluster.Converter.Types
{
    internal interface IComplexType : IType
    {
        byte StructDescrStartFlag { get; }

        byte StructDescrEndFlag { get; }
    }
}
