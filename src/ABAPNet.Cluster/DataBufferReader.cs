using System.Text;

namespace ABAPNet.Cluster
{
    internal sealed class DataBufferReader : IDataBufferContext, IDisposable
    {
        private readonly DataBuffer _dataBuffer;
        private readonly Stream _stream;
        private readonly List<DataBufferSegment> _segments = new List<DataBufferSegment>();
        private DataBufferSegment? _currentSegment;

        private readonly Stack<DataContentContainerType> _dataContentContainers = new Stack<DataContentContainerType>();

        public Stream Stream => _stream;

        public DataBufferSegment? CurrentSegment => _currentSegment;

        public DataBufferConfiguration Configuration => _dataBuffer.Configuration;

        public DataBufferReader(DataBuffer dataBuffer, Stream stream)
        {
            if (!stream.CanRead)
                throw new NotSupportedException("Stream does not support reading");
            if (!stream.CanSeek)
                throw new NotSupportedException("Stream does not support seeking");

            _dataBuffer = dataBuffer;
            _stream = stream;
        }

        public void OpenSegment(DataBufferSegment segment)
        {
            if (_currentSegment != null)
                throw new Exception("An other segment is still open");

            _currentSegment = segment;
            _segments.Add(segment);
        }

        public void CloseSegment()
        {
            if (_currentSegment == null)
                throw new Exception("No segment is currently open");

            while (_dataContentContainers.Count > 0)
                GetClosingDataContentContainer();

            if (_currentSegment.SegmentEndOffset != _stream.Position)
                throw new Exception("Segment length doesn't fit");

            _currentSegment = null;
        }

        public DataContentContainerType GetOpeningDataContentContainer()
        {
            if (_dataContentContainers.TryPeek(out var lastDataContentContainerType))
            {
                if (lastDataContentContainerType == DataContentContainerType.FlatType)
                    return lastDataContentContainerType;

                if (lastDataContentContainerType == DataContentContainerType.StringType)
                    GetClosingDataContentContainer();
            }

            var dataContentContainerType = DataContentContainerTypeExtension.GetFromStartFlag(ReadByte());

            if (dataContentContainerType == DataContentContainerType.FlatType)
                Skip(4);

            _dataContentContainers.Push(dataContentContainerType);

            return dataContentContainerType;
        }

        public DataContentContainerType GetClosingDataContentContainer()
        {
            var dataContentContainerType = DataContentContainerTypeExtension.GetFromEndFlag(ReadByte());

            if (!_dataContentContainers.TryPop(out var lastDataContentContainerType))
                throw new Exception("No content container to be closed");

            if (lastDataContentContainerType != dataContentContainerType)
                throw new Exception("Wrong content container to be closed");

            return dataContentContainerType;
        }

        public DataContentContainerType? GetCurrentDataContentContainer()
        {
            if (_dataContentContainers.Count > 0)
                return _dataContentContainers.Peek();
            return null;
        }

        public bool ReadHeader()
        {
            Span<byte> buffer = stackalloc byte[16];
            Read(buffer);
            return buffer[0] == 0xff && buffer[1] == 0x06 && buffer[2] == 0x02 && buffer[3] == 0x01 && buffer[4] == 0x01 && buffer[5] == 0x02 && buffer[6] == 0x80 && buffer[7] == 0x00 && buffer[8] == 0x34 && buffer[9] == 0x31 && buffer[10] == 0x30 && buffer[11] == 0x33 && buffer[12] == 0x00 && buffer[13] == 0x00 && buffer[14] == 0x00 && buffer[15] == 0x00; 
        }

        public SegmentHeader ReadSegmentHeader()
        {
            SegmentHeader segmentHeader = new SegmentHeader();

            segmentHeader.SegmentStartOffset = _stream.Position;

            segmentHeader.KindFlag = ReadByte();
            segmentHeader.TypeFlag = ReadByte();
            segmentHeader.SpecFlag = ReadByte();

            segmentHeader.DescrByteLength = ReadInvertedInt();
            segmentHeader.SegmentByteLength = ReadInvertedInt();

            byte nameLength = ReadByte();
            _stream.Seek(20, SeekOrigin.Current);

            Span<byte> buffer = stackalloc byte[nameLength * 2];
            Read(buffer);

            segmentHeader.SegmentName = Encoding.Unicode.GetString(buffer);

            segmentHeader.SegmentEndOffset = segmentHeader.SegmentStartOffset + segmentHeader.SegmentByteLength;

            return segmentHeader;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public int Read(Span<byte> buffer)
        {
            return _stream.Read(buffer);
        }

        public byte ReadByte()
        {
            Span<byte> buffer = stackalloc byte[1];
            _stream.Read(buffer);
            return buffer[0];
        }

        public int ReadInvertedInt()
        {
            Span<byte> buffer = stackalloc byte[4];
            _stream.Read(buffer);
            buffer.Reverse();
            return BitConverter.ToInt32(buffer);
        }

        public void Skip(int offset)
        {
            _stream.Position += offset;
        }

        public void SkipByte()
        {
            _stream.Position++;
        }

        public byte PeekByte()
        {
            Span<byte> buffer = stackalloc byte[1];
            _stream.ReadExactly(buffer);
            _stream.Position--;
            return buffer[0];
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public struct SegmentHeader
        {
            public byte KindFlag { get; set; }

            public byte TypeFlag { get; set; }

            public byte SpecFlag { get; set; }

            public int DescrByteLength { get; set; }

            public long SegmentByteLength { get; set; }

            public long SegmentStartOffset { get; set; }

            public long SegmentEndOffset { get; set; }

            public string SegmentName { get; set; }
        }
    }
}
