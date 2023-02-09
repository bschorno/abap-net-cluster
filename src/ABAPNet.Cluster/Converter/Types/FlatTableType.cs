namespace ABAPNet.Cluster.Converter.Types
{
    internal class FlatTableType : ITableType, IType
    {
        public byte KindFlag => 0x03;

        public byte TypeFlag => 0x0e;

        public byte SpecFlag => 0x00;

        public byte StructDescrStartFlag => 0xad;

        public byte StructDescrEndFlag => 0xae;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public IType GetLineType()
        {
            return new FlatStructType();
        }
    }

    internal class FlatTableType<T> : ITableType, IType where T : IFlatType
    {
        private T _lineType;

        public byte KindFlag => 0x03;

        public byte TypeFlag => _lineType.TypeFlag;

        public byte SpecFlag => _lineType.SpecFlag;

        public byte StructDescrStartFlag => 0xad;

        public byte StructDescrEndFlag => 0xae;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public FlatTableType(T lineType)
        {
            _lineType = lineType;
        }

        public IType GetLineType()
        {
            return _lineType;
        }
    }
}
