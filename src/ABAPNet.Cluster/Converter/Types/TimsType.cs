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

            Span<char> year = stackalloc char[4];
            Span<char> month = stackalloc char[2];
            Span<char> day = stackalloc char[2];

            ReadOnlySpan<char> format = stackalloc char[4] { '0', '0', '0', '0' };

            if (data is DateOnly dateOnly)
            {
                dateOnly.Year.TryFormat(year, out int charsWritten, format);
                dateOnly.Month.TryFormat(month, out charsWritten, format[..2]);
                dateOnly.Day.TryFormat(day, out charsWritten, format[..2]);
            }
            else if (data is DateTime dateTime)
            {
                dateTime.Year.TryFormat(year, out int charsWritten, format);
                dateTime.Month.TryFormat(month, out charsWritten, format[..2]);
                dateTime.Day.TryFormat(day, out charsWritten, format[..2]);
            }
            else
                throw new InvalidTypeException(data, typeof(DateOnly), typeof(DateTime));

            context.Configuration.CodePage.Encoding.GetBytes(year, buffer[0..8]);
            context.Configuration.CodePage.Encoding.GetBytes(month, buffer[8..12]);
            context.Configuration.CodePage.Encoding.GetBytes(day, buffer[12..16]);

            return buffer;
        }

        public void SetBytes(ref object data, ReadOnlySpan<byte> buffer, IDataBufferContext context)
        {
            Span<char> chars = stackalloc char[8];

            context.Configuration.CodePage.Encoding.GetChars(buffer, chars);

            _ = int.TryParse(chars[0..4], out int year);
            _ = int.TryParse(chars[4..6], out int month);
            _ = int.TryParse(chars[6..8], out int day);

            if (data is DateOnly dateOnly)
            {
                data = new DateOnly(year, month, day);
            }
            else if (data is DateTime dateTime)
            {
                data = new DateTime(year, month, day);
            }
            else
                throw new InvalidTypeException(data, typeof(DateOnly), typeof(DateTime));
        }
    }
}
