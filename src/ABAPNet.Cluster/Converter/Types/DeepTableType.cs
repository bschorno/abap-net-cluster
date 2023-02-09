namespace ABAPNet.Cluster.Converter.Types
{
    internal class DeepTableType : ITableType, IType
    {
        public byte KindFlag => 0x06;

        public byte TypeFlag => 0x0f;

        public byte SpecFlag => 0x00;

        public byte StructDescrStartFlag => 0xad;

        public byte StructDescrEndFlag => 0xae;

        public int StructDescrByteLength => 8; 

        public int AlignmentFactor => 8;

        public IType GetLineType()
        {
            return new DeepStructType();
        }
    }

    internal class DeepTableType<T> : ITableType, IType where T : IStringType
    {
        private T _lineType;

        public byte KindFlag => 0x06;

        public byte TypeFlag => _lineType.TypeFlag;

        public byte SpecFlag => _lineType.SpecFlag;

        public byte StructDescrStartFlag => 0xad;

        public byte StructDescrEndFlag => 0xae;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public DeepTableType(T lineType)
        {
            _lineType = lineType;
        }

        public IType GetLineType()
        {
            return _lineType;
        }
    }
}
