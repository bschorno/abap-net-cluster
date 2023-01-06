namespace ABAPNet.Cluster.Converter.Types
{
    internal class FloatType : IFlatType, IType
    {
        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x07;

        public byte SpecFlag => 0x00;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => 8;

        public int AlignmentFactor => 8;

        public ReadOnlySpan<byte> GetBytes(object? data)
        {
            Span<byte> buffer = new Span<byte>(new byte[StructDescrByteLength]);

            if (data == null)
                data = new double();

            var result = data switch
            {
                float value => BitConverter.TryWriteBytes(buffer, (double)value),
                double value => BitConverter.TryWriteBytes(buffer, value),
                _ => throw new InvalidTypeException(data, typeof(float), typeof(double))
            };

            if (!result)
                throw new Exception("Unexpected exception");

            return buffer;
        }
    }
}
