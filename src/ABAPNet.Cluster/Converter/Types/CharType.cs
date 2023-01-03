using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class CharType : IFlatType, IType
    {
        private int _length;

        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => _length * 2;

        public int AlignmentFactor => 2;

        public int Length => _length;

        public CharType(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            _length = length;
        }

        public ReadOnlySpan<byte> GetBytes(object data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            if (data is not string stringValue)
            {
                if (_length == 1 && data is char charValue)
                    stringValue = charValue.ToString();
                else
                    throw new Exception("Invalid data type");
            }

            if (stringValue.Length > _length)
                stringValue = stringValue.Substring(0, _length);
            while (stringValue.Length < _length)
                stringValue += " ";

            Encoding.Unicode.GetBytes(stringValue, buffer);
            return buffer;
        }
    }
}
