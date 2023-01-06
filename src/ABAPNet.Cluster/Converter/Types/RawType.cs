using System.Collections;
using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class RawType : IFlatType, IType
    {
        private int _length;

        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x04;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => _length;

        public int AlignmentFactor => 1;

        public int Length => _length;

        public RawType(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            _length = length;
        }

        public ReadOnlySpan<byte> GetBytes(object? data)
        {
            if (data == null)
                return new Span<byte>(new byte[StructDescrByteLength]);

            Type type = data.GetType();

            if (!type.IsArray && data is not IList list && type.GetElementType() != typeof(byte))
            {
                if (_length == 1 && data is byte byteValue)
                    return new Span<byte>(new byte[] { byteValue });
                throw new InvalidTypeException(data, typeof(byte[]), typeof(byte));
            }

            var rawData = (byte[])data;
            if (rawData.Length >= _length)
            {
                ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(rawData);
                return span.Slice(0, _length);
            }
            else
            {
                Span<byte> buffer = new Span<byte>(new byte[_length]);
                rawData.CopyTo(buffer);
                return buffer;
            }
        }
    }
}
