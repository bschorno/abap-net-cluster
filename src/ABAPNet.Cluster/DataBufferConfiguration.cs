namespace ABAPNet.Cluster
{
    public class DataBufferConfiguration
    {
        private readonly CodePage _codePage;
        private readonly Endian _endian;

        private readonly bool _importIgnoreMissingSegments;
        private readonly bool _importIgnoreSegmentTypeMismatch;

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

        public bool ImportIgnoreMissingSegments
        {
            get { return _importIgnoreMissingSegments; }
            init { _importIgnoreMissingSegments = value; }
        }

        public bool ImportIgnoreSegmentTypeMismatch
        {
            get { return _importIgnoreSegmentTypeMismatch; }
            init { _importIgnoreSegmentTypeMismatch = value; }
        }

        public DataBufferConfiguration(CodePage codePage, Endian endian)
        {
            _codePage = codePage;
            _endian = endian;
        }
    }
}
