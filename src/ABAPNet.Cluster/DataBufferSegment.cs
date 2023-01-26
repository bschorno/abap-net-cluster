using ABAPNet.Cluster.Converter.Descriptors;
using System.IO;
using System.Reflection;
using System.Transactions;

namespace ABAPNet.Cluster
{
    internal class DataBufferSegment
    {
        private readonly string _name;
        private readonly Descriptor _descriptor;
        private PropertyInfo _propertyInfo;

        private long _segmentStartOffset = 0;
        private long _segmentEndOffset = 0;

        public string Name => _name;

        public Descriptor Descriptor => _descriptor;

        public PropertyInfo PropertyInfo => _propertyInfo;

        public long SegmentStartOffset
        {
            get { return _segmentStartOffset; }
            set { _segmentStartOffset = value; }
        }

        public long SegmentEndOffset
        {
            get { return _segmentEndOffset; }
            set { _segmentEndOffset = value; }
        }

        public int SegmentByteLength
        {
            get
            {
                if (_segmentEndOffset <= _segmentStartOffset)
                    return 0;
                return (int)(_segmentEndOffset - _segmentStartOffset);
            }
        }

        public DataBufferSegment(string name, Descriptor descriptor, PropertyInfo propertyInfo)
        {
            _name = name;
            _descriptor = descriptor;
            _propertyInfo = propertyInfo;
        }
    }
}
