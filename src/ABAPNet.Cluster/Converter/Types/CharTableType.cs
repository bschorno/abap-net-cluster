using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class CharTableType : ITableType, IType
    {
        private int _length;

        public byte KindFlag => 0x03;

        public byte TypeFlag => 0x00;

        public byte SpecFlag => 0x00;

        public byte StructDescrStartFlag => 0xad;

        public byte StructDescrEndFlag => 0xae;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public int Length => _length;

        public CharTableType(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            _length = length;
        }

        public IType GetLineType()
        {
            return new CharType(_length);
        }
    }
}
