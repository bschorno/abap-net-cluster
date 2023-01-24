using System.Collections;
using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class RawStringType : IStringType, IType
    {
        public byte KindFlag => 0x07;

        public byte TypeFlag => 0x14;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 4;

        public ReadOnlySpan<byte> GetBytes(object? data, DataBufferConfiguration configuration)
        {
            if (data == null)
                return ReadOnlySpan<byte>.Empty;

            Type type = data.GetType();

            if (!type.IsArray && data is not IList list && type.GetElementType() != typeof(byte))
                throw new InvalidTypeException(data, typeof(byte[]));

            return new ReadOnlySpan<byte>(data as byte[]);
        }
    }
}
