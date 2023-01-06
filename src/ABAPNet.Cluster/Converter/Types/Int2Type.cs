namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int2Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x09;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 2;

        public int AlignmentFactor => 2;

        public ReadOnlySpan<byte> GetBytes(object? data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            var result = data switch
            {
                short value => BitConverter.TryWriteBytes(buffer, value),
                ushort value => BitConverter.TryWriteBytes(buffer, value),
                _ => throw new InvalidTypeException(data, typeof(short), typeof(ushort))
            };

            if (!result)
                throw new Exception("Unexpected exception");

            return buffer;
        }
    }
}
