using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class StringType : IStringType, IType
    {
        public byte KindFlag => 0x07;

        public byte TypeFlag => 0x13;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 4;

        public ReadOnlySpan<byte> GetBytes(object? data, IDataBufferContext context)
        {
            if (data == null)
                return ReadOnlySpan<byte>.Empty;

            if (data is not string stringValue)
                throw new InvalidTypeException(data, typeof(string));

            Span<byte> buffer = new Span<byte>(new byte[stringValue.Length * 2]);

            context.Configuration.CodePage.Encoding.GetBytes(stringValue, buffer);
            return buffer;
        }

        public void SetBytes(ref object data, ReadOnlySpan<byte> buffer, IDataBufferContext context)
        {
            if (data is not string)
                throw new InvalidTypeException(data, typeof(string));

            Span<char> chars = new char[buffer.Length / 2];

            context.Configuration.CodePage.Encoding.GetChars(buffer, chars);

            data = chars.ToString();
        }
    }
}
