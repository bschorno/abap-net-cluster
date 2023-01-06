namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int8Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x1b;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public ReadOnlySpan<byte> GetBytes(object? data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            var result = data switch
            {
                long value => BitConverter.TryWriteBytes(buffer, value),
                ulong value => BitConverter.TryWriteBytes(buffer, value),
                _ => throw new InvalidTypeException(data, typeof(long), typeof(ulong))
            };

            if (!result)
                throw new Exception("Unexpected exception");

            return buffer;
        }
    }
}
