namespace ABAPNet.Cluster.Converter.Types
{
    internal class DeepStructType : IStructType, IType
    {
        public byte KindFlag => 0x05;

        public byte TypeFlag => 0x0f;

        public byte SpecFlag => 0x00;

        public byte StructDescrStartFlag => 0xab;

        public byte StructDescrEndFlag => 0xac;
    }
}
