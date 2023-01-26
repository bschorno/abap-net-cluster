using ABAPNet.Cluster.Converter.Types;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace ABAPNet.Cluster.Benchmark
{
    [MemoryDiagnoser(false)]
    public class BenchmarkTypeWriting
    {
        private struct DataBufferContext : IDataBufferContext
        {
            public DataBufferConfiguration Configuration { get; set; }

            public DataBufferSegment? CurrentSegment { get; set; }
        }

        private DataBufferContext _context;

        private CharType _charType;
        private StringType _stringType;

        [GlobalSetup]
        public void Setup()
        {
            _context = new DataBufferContext()
            {
                Configuration = new DataBufferConfiguration(CodePage.UTF16LE, Endian.LittleEndian)
            };

            _charType = new CharType(10);
            _stringType = new StringType();
        }

        [Benchmark]
        public void CharExactCharacters()
        {
            var bytes = _charType.GetBytes("0123456789", _context);
        }

        [Benchmark]
        public void CharLessCharacters()
        {
            var bytes = _charType.GetBytes("01245", _context);
        }

        [Benchmark]
        public void CharMoreCharacters()
        {
            var bytes = _charType.GetBytes("0124567890123456789", _context);
        }

        [Benchmark]
        public void CharWhenNull()
        {
            var bytes = _charType.GetBytes(null, _context);
        }

        [Benchmark]
        public void String10Characters()
        {
            var bytes = _charType.GetBytes("0123456789", _context);
        }

        [Benchmark]
        public void String20Characters()
        {
            var bytes = _charType.GetBytes("01234567890123456789", _context);
        }

        [Benchmark]
        public void String40Characters()
        {
            var bytes = _charType.GetBytes("0123456789012345678901234567890123456789", _context);
        }
    }
}
