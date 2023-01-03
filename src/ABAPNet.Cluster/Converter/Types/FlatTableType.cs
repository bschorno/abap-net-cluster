namespace ABAPNet.Cluster.Converter.Types
{
    internal class FlatTableType : ITableType, IType
    {
        public byte KindFlag => 0x03;

        public byte TypeFlag => 0x0e;

        public byte StructDescrStartFlag => 0xad;

        public byte StructDescrEndFlag => 0xae;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public IType GetLineType()
        {
            return new FlatStructType();
        }
    }
}
