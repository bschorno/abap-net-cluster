namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int4Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x08;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 4;

        public int AlignmentFactor => 4;

        public ReadOnlySpan<byte> GetBytes(object data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            if (data is not int intValue)
                throw new Exception("Invalid data type");

            BitConverter.TryWriteBytes(buffer, intValue);
            return buffer;
        }
    }
}
