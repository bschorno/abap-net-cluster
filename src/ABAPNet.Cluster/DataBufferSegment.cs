using System.IO;
using System.Transactions;

namespace ABAPNet.Cluster
{
    internal class DataBufferSegment
    {
        public enum DataContentContainerType : byte
        {
            FlatType = 0x01, 
            StringType = 0x02,
            TableType = 0x04
        }

        private readonly DataBufferWriter _writer;
        private readonly string _name;

        private long _segmentStartOffset = 0;
        private long _segmentEndOffset = 0;

        private readonly Stack<DataContentContainerType> _dataContentContainers = new Stack<DataContentContainerType>();
        private long _flatDataContainerByteOffset = 0;
        
        public string Name => _name;

        public long SegmentStartOffset => _segmentStartOffset;

        public long SegmentEndOffset => _segmentEndOffset;

        public DataBufferSegment(DataBufferWriter writer, string name)
        {
            _writer = writer;
            _name = name;

            _segmentStartOffset = _writer.Stream.Position;
        }

        public void Close()
        {
            while (_dataContentContainers.Count > 0)
                CloseDataContenetContainer(_dataContentContainers.Peek());

            _segmentEndOffset = _writer.Stream.Position;
            _writer.WriteInvertedIntAt((int)(_segmentEndOffset - _segmentStartOffset), _segmentStartOffset + 7);
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
                    CloseDataContenetContainer(lastDataContentContainerType);
            }

            _dataContentContainers.Push(dataContentContainerType);

            _writer.WriteByte(GetDataContentContainerFlags(dataContentContainerType).start);

            if (dataContentContainerType == DataContentContainerType.FlatType)
            {
                _flatDataContainerByteOffset = _writer.Stream.Position;
                _writer.Write(new byte[4]);
            }
        }

        public void CloseDataContenetContainer(DataContentContainerType dataContentContainerType)
        {
            while (_dataContentContainers.Count > 0 && _dataContentContainers.Peek() != dataContentContainerType)
                CloseDataContenetContainer(_dataContentContainers.Peek());

            if (!_dataContentContainers.TryPop(out var lastDataContentContainerType) || 
                lastDataContentContainerType != dataContentContainerType)
                throw new Exception("Content container can't be closed, because it is not the last one opened");

            if (dataContentContainerType == DataContentContainerType.FlatType)
            {
                long flatDataContainerByteLength = _writer.Stream.Position - _flatDataContainerByteOffset - 4;
                _writer.WriteInvertedIntAt((int)flatDataContainerByteLength, _flatDataContainerByteOffset);
            }

            _writer.WriteByte(GetDataContentContainerFlags(dataContentContainerType).end);
        }

        public DataContentContainerType? GetCurrentDataContentContainer()
        {
            if (_dataContentContainers.Count > 0)
                return _dataContentContainers.Peek();
            return null;
        }

        private (byte start, byte end) GetDataContentContainerFlags(DataContentContainerType dataContentContainerType)
            => dataContentContainerType switch
            {
                DataContentContainerType.FlatType => (0xbc, 0xbd),
                DataContentContainerType.StringType => (0xca, 0xcb),
                DataContentContainerType.TableType => (0xbe, 0xbf),
                _ => throw new Exception("Invalid enumerator")
            };
    }
}
