namespace ABAPNet.Cluster.Converter.Types
{
    internal interface IType
    {
        byte KindFlag { get; }

        byte TypeFlag { get; }

        byte SpecFlag { get; }
    }
}
