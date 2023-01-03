using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class NumcType : IFlatType, IType
    {
        private int _length;

        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x06;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => _length * 2;

        public int AlignmentFactor => 2;

        public int Length => _length;

        public NumcType(int length)
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
                stringValue = data switch
                {
                    byte byteValue => byteValue.ToString(),
                    short shortValue => shortValue.ToString(),
                    int intValue => intValue.ToString(),
                    long longValue => longValue.ToString(),
                    sbyte sbyteValue => sbyteValue.ToString(),
                    ushort ushortValue => ushortValue.ToString(),
                    uint uintValue => uintValue.ToString(),
                    ulong ulongValue => ulongValue.ToString(),
                    _ => throw new Exception("Invalid data type")
                };
            }

            if (stringValue.Length > _length)
                stringValue = stringValue.Substring(0, _length);
            while (stringValue.Length < _length)
                stringValue = "0" + stringValue;

            Encoding.Unicode.GetBytes(stringValue, buffer);
            return buffer;
        }
    }
}
