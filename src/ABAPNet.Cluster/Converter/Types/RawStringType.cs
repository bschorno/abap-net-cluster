using System.Collections;
using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class RawStringType : IStringType, IType
    {
        public byte KindFlag => 0x07;

        public byte TypeFlag => 0x14;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 4;

        public ReadOnlySpan<byte> GetBytes(object data)
        {
            if (data == null)
                return ReadOnlySpan<byte>.Empty;

            Type type = data.GetType();

            if (!type.IsArray && data is not IList list)
                throw new Exception("Invalid data type");

            if (type.GetElementType() != typeof(byte))
                throw new Exception("Invalid data type");

            return new ReadOnlySpan<byte>(data as byte[]);
        }
    }
}
