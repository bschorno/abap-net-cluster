namespace ABAPNet.Cluster
{
    public class DataBufferConfiguration
    {
        private readonly CodePage _codePage;
        private readonly Endian _endian;

        public CodePage CodePage
        {
            get { return _codePage; }
            init { _codePage = value; }
        }

        public Endian Endian
        {
            get { return _endian; }
            init { _endian = value; }
        }

        public DataBufferConfiguration(CodePage codePage, Endian endian)
        {
            _codePage = codePage;
            _endian = endian;
        }
    }
}
