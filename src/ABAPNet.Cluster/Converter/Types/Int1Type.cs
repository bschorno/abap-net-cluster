namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int1Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x0a;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 1;

        public int AlignmentFactor => 1;

        public ReadOnlySpan<byte> GetBytes(object data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            if (data is not byte byteValue)
                throw new Exception("Invalid data type");

            buffer[0] = byteValue;
            return buffer;
        }
    }
}
