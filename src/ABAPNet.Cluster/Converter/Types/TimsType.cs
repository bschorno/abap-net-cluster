using ABAPNet.Cluster.Utils;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class TimsType : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x01;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 16;

        public int AlignmentFactor => 2;

        public TimsType()
        {

        }

        private const char ZeroChar = '0';

        public ReadOnlySpan<byte> GetBytes(object? data, IDataBufferContext context)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
            {
                Span<byte> tempBuffer = stackalloc byte[2];
                context.Configuration.CodePage.Encoding.GetBytes(ZeroChar, tempBuffer);
                for (int i = 0; i < buffer.Length; i += 2)
                {
                    buffer[i] = tempBuffer[0];
                    buffer[i + 1] = tempBuffer[1];
                }

                return buffer;
            }

            Span<char> hour = stackalloc char[2];
            Span<char> minute = stackalloc char[2];
            Span<char> second = stackalloc char[2];

            ReadOnlySpan<char> format = stackalloc char[2] { '0', '0' };

            if (data is TimeOnly timeOnly)
            {
                timeOnly.Hour.TryFormat(hour, out int charsWritten, format);
                timeOnly.Minute.TryFormat(minute, out charsWritten, format);
                timeOnly.Second.TryFormat(second, out charsWritten, format);
            }
            else if (data is DateTime dateTime)
            {
                dateTime.Hour.TryFormat(hour, out int charsWritten, format);
                dateTime.Minute.TryFormat(minute, out charsWritten, format);
                dateTime.Second.TryFormat(second, out charsWritten, format);
            }
            else
                throw new InvalidTypeException(data, typeof(TimeOnly), typeof(DateTime));

            context.Configuration.CodePage.Encoding.GetBytes(hour, buffer[0..4]);
            context.Configuration.CodePage.Encoding.GetBytes(minute, buffer[4..8]);
            context.Configuration.CodePage.Encoding.GetBytes(second, buffer[8..12]);

            return buffer;
        }

        public void SetBytes(ref object data, ReadOnlySpan<byte> buffer, IDataBufferContext context)
        {
            Span<char> chars = stackalloc char[6];

            context.Configuration.CodePage.Encoding.GetChars(buffer, chars);

            _ = int.TryParse(chars[0..2], out int hour);
            _ = int.TryParse(chars[2..4], out int minute);
            _ = int.TryParse(chars[4..6], out int second);

            if (data is TimeOnly timeOnly)
            {
                data = new TimeOnly(hour, minute, second);
            }
            else if (data is DateTime dateTime)
            {
                data = new DateTime(0, 0, 0, hour, minute, second);
            }
            else
                throw new InvalidTypeException(data, typeof(TimeOnly), typeof(DateTime));
        }
    }
}
