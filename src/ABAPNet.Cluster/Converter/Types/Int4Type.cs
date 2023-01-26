namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int4Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x08;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 4;

        public int AlignmentFactor => 4;

        public ReadOnlySpan<byte> GetBytes(object? data, IDataBufferContext context)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            var result = data switch
            {
                int value => BitConverter.TryWriteBytes(buffer, value),
                uint value => BitConverter.TryWriteBytes(buffer, value),
                _ => throw new InvalidTypeException(data, typeof(int), typeof(uint))
            };

            if (!result)
                throw new Exception("Unexpected exception");

            return buffer;
        }

        public void SetBytes(ref object data, ReadOnlySpan<byte> buffer, IDataBufferContext context)
        {
            data = data switch
            {
                int => BitConverter.ToInt32(buffer),
                uint => BitConverter.ToUInt32(buffer),
                _ => throw new InvalidTypeException(data, typeof(int), typeof(uint))
            };
        }
    }
}
