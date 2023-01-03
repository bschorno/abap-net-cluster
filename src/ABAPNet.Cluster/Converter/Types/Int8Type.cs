namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int8Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x1b;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public ReadOnlySpan<byte> GetBytes(object data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            if (data is not long longValue)
                throw new Exception("Invalid data type");

            BitConverter.TryWriteBytes(buffer, longValue);
            return buffer;
        }
    }
}
