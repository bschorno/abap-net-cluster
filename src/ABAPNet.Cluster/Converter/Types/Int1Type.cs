namespace ABAPNet.Cluster.Converter.Types
{
    internal class Int1Type : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x0a;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 1;

        public int AlignmentFactor => 1;

        public ReadOnlySpan<byte> GetBytes(object? data, IDataBufferContext context)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                return buffer;

            buffer[0] = data switch
            {
                byte byteValue => byteValue,
                sbyte sbyteValue => (byte)sbyteValue,
                _ => throw new InvalidTypeException(data, typeof(byte), typeof(sbyte))
            };

            return buffer;
        }

        public void SetBytes(ref object data, ReadOnlySpan<byte> buffer, IDataBufferContext context)
        {
            data = data switch
            {
                byte => buffer[0],
                sbyte => (sbyte)buffer[0],
                _ => throw new InvalidTypeException(data, typeof(byte), typeof(sbyte))
            };
        }
    }
}
