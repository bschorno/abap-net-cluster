namespace ABAPNet.Cluster.Converter.Types
{
    internal interface ISimpleType : IType, IDefinedType, IAlignedType
    {
        byte StructDescrFlag { get; }

        ReadOnlySpan<byte> GetBytes(object? data, IDataBufferContext context);

        void SetBytes(ref object data, ReadOnlySpan<byte> buffer, IDataBufferContext context);
    }
}
