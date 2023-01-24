using System;

namespace ABAPNet.Cluster
{
    internal class DataBufferWriter
    {
        private readonly DataBuffer _dataBuffer;
        private readonly Stream _stream;
        private readonly List<DataBufferSegment> _segments = new List<DataBufferSegment>();
        private DataBufferSegment? _currentSegment;

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

        public DataBufferSegment OpenSegment(string segmentName)
        {
            _currentSegment = new DataBufferSegment(this, segmentName);
            _segments.Add(_currentSegment);

            return _currentSegment;
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
    }
}
