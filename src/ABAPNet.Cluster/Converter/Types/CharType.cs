using ABAPNet.Cluster.Utils;
using System.Text;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class CharType : IFlatType, IType
    {
        private int _length;

        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x00;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => _length * 2;

        public int AlignmentFactor => 2;

        public int Length => _length;

        public CharType(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            _length = length;
        }

        private const char SpaceChar = ' ';

        public ReadOnlySpan<byte> GetBytes(object? data, IDataBufferContext context)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
            {
                FillBufferWithSpace(context.Configuration.CodePage.Encoding, 0, _length, buffer);
            }
            else if (data is char charValue && _length == 1)
            {
                context.Configuration.CodePage.Encoding.GetBytes(charValue, buffer);   
            }
            else if (data is string stringValue)
            {
                if (stringValue.Length >= _length)
                {
                    ReadOnlySpan<char> chars = stringValue;
                    context.Configuration.CodePage.Encoding.GetBytes(chars.Slice(0, _length), buffer);
                }
                else
                {
                    context.Configuration.CodePage.Encoding.GetBytes(stringValue, buffer);
                    FillBufferWithSpace(context.Configuration.CodePage.Encoding, stringValue.Length, _length, buffer);
                }
            }
            else
                throw new InvalidTypeException(data, typeof(string), typeof(char));

            return buffer;
        }

        public void SetBytes(ref object data, ReadOnlySpan<byte> buffer, IDataBufferContext context)
        {
            Span<char> chars = stackalloc char[_length];

            context.Configuration.CodePage.Encoding.GetChars(buffer, chars);

            if (data is char charValue && _length == 1)
            {
                data = chars[0];
            }
            else if (data is string)
            {
                data = chars.ToString();
            }
            else
                throw new InvalidTypeException(data, typeof(string), typeof(char));
        }

        private void FillBufferWithSpace(Encoding encoding, int start, int end, Span<byte> buffer)
        {
            Span<byte> tempBuffer = stackalloc byte[2];
            encoding.GetBytes(SpaceChar, tempBuffer);

            for (int i = start * 2; i < end * 2; i += 2)
            {
                buffer[i] = tempBuffer[0];
                buffer[i + 1] = tempBuffer[1];
            }
        }
    }
}
