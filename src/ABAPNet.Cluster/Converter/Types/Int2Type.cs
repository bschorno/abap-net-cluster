namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int2Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x09;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 2;

        public int AlignmentFactor => 2;

        public ReadOnlySpan<byte> GetBytes(object data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            if (data is not short shortValue)
                throw new Exception("Invalid data type");

            BitConverter.TryWriteBytes(buffer, shortValue);
            return buffer;
        }
    }
}
