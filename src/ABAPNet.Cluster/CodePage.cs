using System.Text;

namespace ABAPNet.Cluster
{
    public class CodePage
    {
        public static CodePage UTF16BE = new("4102", Encoding.BigEndianUnicode, Endian.BigEndian);
        public static CodePage UTF16LE = new("4103", Encoding.Unicode, Endian.LittleEndian);

        private readonly string _code;
        private readonly Encoding _encoding;
        private readonly Endian _endian;

        public string Code => _code;

        public Encoding Encoding => _encoding;

        public Endian Endian => _endian;

        private CodePage(string code, Encoding encoding)
            : this(code, encoding, Endian.None) { }

        private CodePage(string code, Encoding encoding, Endian endian)
            => (_code, _encoding, _endian) = (code, encoding, endian);
    }
}
