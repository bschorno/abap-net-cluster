using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class NumcType : IFlatType, IType
    {
        private int _length;

        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x06;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => _length * 2;

        public int AlignmentFactor => 2;

        public int Length => _length;

        public NumcType(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            _length = length;
        }

        public ReadOnlySpan<byte> GetBytes(object? data, DataBufferConfiguration configuration)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                data = new string('0', _length);

            if (data is not string stringValue)
            {
                stringValue = data switch
                {
                    byte value => value.ToString(),
                    short value => value.ToString(),
                    int value => value.ToString(),
                    long value => value.ToString(),
                    sbyte value => value.ToString(),
                    ushort value => value.ToString(),
                    uint value => value.ToString(),
                    ulong value => value.ToString(),
                    _ => throw new InvalidTypeException(data, typeof(byte), typeof(short), typeof(int), typeof(long), typeof(sbyte), typeof(ushort), typeof(uint), typeof(ulong), typeof(string))
                };
            }

            foreach (var c in stringValue)
                if (c < '0' || c > '9')
                    throw new InvalidValueException(stringValue, "Value can only contains digits from 0-9");

            if (stringValue.Length > _length)
                stringValue = stringValue.Substring(0, _length);
            while (stringValue.Length < _length)
                stringValue = "0" + stringValue;

            Encoding.Unicode.GetBytes(stringValue, buffer);
            return buffer;
        }
    }
}
