namespace ABAPNet.Cluster.Converter.Types
{
    internal interface ITableType : IComplexType, IDefinedType, IAlignedType
    {
        IType GetLineType();
    }
}
