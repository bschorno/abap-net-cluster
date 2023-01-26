using ABAPNet.Cluster.Converter.Descriptors;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Xml.Linq;

namespace ABAPNet.Cluster
{
    internal sealed class DataBufferWriter : IDataBufferContext, IDisposable
    {
        private readonly DataBuffer _dataBuffer;
        private readonly Stream _stream;
        private readonly List<DataBufferSegment> _segments = new List<DataBufferSegment>();
        private DataBufferSegment? _currentSegment;

        private readonly Stack<DataContentContainerType> _dataContentContainers = new Stack<DataContentContainerType>();
        private long _flatDataContainerByteOffset = 0;

        public Stream Stream => _stream;

        public DataBufferSegment? CurrentSegment => _currentSegment;

        public DataBufferConfiguration Configuration => _dataBuffer.Configuration;

        public DataBufferWriter(DataBuffer dataBuffer, Stream stream)
        {
            if (!stream.CanWrite)
                throw new NotSupportedException("Stream does not support writing");
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

            _currentSegment.SegmentStartOffset = _stream.Position;
        }

        public void CloseSegment()
        {
            if (_currentSegment == null)
                throw new Exception("No segemnt is currently open");

            while (_dataContentContainers.Count > 0)
                CloseDataContentContainer(_dataContentContainers.Peek());

            _currentSegment.SegmentEndOffset = _stream.Position;
            WriteInvertedIntAt(_currentSegment.SegmentByteLength, _currentSegment.SegmentStartOffset + 7);

            _currentSegment = null;
        }

        public void OpenDataContentContainer(DataContentContainerType dataContentContainerType)
        {
            if (_dataContentContainers.TryPeek(out var lastDataContentContainerType))
            {
                if (lastDataContentContainerType == DataContentContainerType.FlatType &&
                    dataContentContainerType == DataContentContainerType.FlatType)
                    return;

                if (lastDataContentContainerType == DataContentContainerType.FlatType ||
                    lastDataContentContainerType == DataContentContainerType.StringType)
                    CloseDataContentContainer(lastDataContentContainerType);
            }

            _dataContentContainers.Push(dataContentContainerType);

            WriteByte(dataContentContainerType.GetFlags().Start);

            if (dataContentContainerType == DataContentContainerType.FlatType)
            {
                _flatDataContainerByteOffset = _stream.Position;
                Write(new byte[4]);
            }
        }

        public void CloseDataContentContainer(DataContentContainerType dataContentContainerType)
        {
            while (_dataContentContainers.Count > 0 && _dataContentContainers.Peek() != dataContentContainerType)
                CloseDataContentContainer(_dataContentContainers.Peek());

            if (!_dataContentContainers.TryPop(out var lastDataContentContainerType) ||
                lastDataContentContainerType != dataContentContainerType)
                throw new Exception("Content container can't be closed, because it is not the last one opened");

            if (dataContentContainerType == DataContentContainerType.FlatType)
            {
                long flatDataContainerByteLength = _stream.Position - _flatDataContainerByteOffset - 4;
                WriteInvertedIntAt((int)flatDataContainerByteLength, _flatDataContainerByteOffset);
            }

            WriteByte(dataContentContainerType.GetFlags().End);
        }

        public DataContentContainerType? GetCurrentDataContentContainer()
        {
            if (_dataContentContainers.Count > 0)
                return _dataContentContainers.Peek();
            return null;
        }

        public void WriteHeader()
        {
            Write(new byte[16] { 0xff, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33, 0x00, 0x00, 0x00, 0x00 }, 0, 16);
        }

        public void WriteSegmentHeader()
        {
            if (_currentSegment == null)
                throw new Exception("No segemnt is currently open");

            WriteByte(_currentSegment.Descriptor.Type.KindFlag);
            WriteByte(_currentSegment.Descriptor.Type.TypeFlag);
            WriteByte(_currentSegment.Descriptor.Type.SpecFlag);

            WriteInvertedInt(_currentSegment.Descriptor.GetDescrByteLength());
            Write(new byte[4]);

            Write(BitConverter.GetBytes(_currentSegment.Name.Length), 0, 1);
            Write(new byte[20]);
            Write(Encoding.Unicode.GetBytes(_currentSegment.Name));
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public void Write(ReadOnlySpan<byte> buffer)
        {
            _stream.Write(buffer);
        }

        public void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        public void WriteInvertedInt(int value)
        {
            Span<byte> buffer = BitConverter.GetBytes(value);
            buffer.Reverse();
            _stream.Write(buffer);
        }

        public void WriteAt(byte[] buffer, int offset, int count, long position)
        {
            if (position > _stream.Length)
                throw new ArgumentOutOfRangeException(nameof(position));

            var tmpPosition = _stream.Position;
            _stream.Position = position;
            Write(buffer, offset, count);
            _stream.Position = tmpPosition;
        }

        public void WriteAt(ReadOnlySpan<byte> buffer, long position)
        {
            if (position > _stream.Length)
                throw new ArgumentOutOfRangeException(nameof(position));

            var tmpPosition = _stream.Position;
            _stream.Position = position;
            Write(buffer);
            _stream.Position = tmpPosition;
        }

        public void WriteByteAt(byte value, long position)
        {
            if (position > _stream.Length)
                throw new ArgumentOutOfRangeException(nameof(position));

            var tmpPosition = _stream.Position;
            _stream.Position = position;
            WriteByte(value);
            _stream.Position = tmpPosition;
        }

        public void WriteInvertedIntAt(int value, long position)
        {
            if (position > _stream.Length)
                throw new ArgumentOutOfRangeException(nameof(position));

            var tmpPosition = _stream.Position;
            _stream.Position = position;
            WriteInvertedInt(value);
            _stream.Position = tmpPosition;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);   
        }
    }
}
