namespace ABAPNet.Cluster.Converter.Types
{
    internal class FlatStructType : IStructType, IType
    {
        public byte KindFlag => 0x02;

        public byte TypeFlag => 0x0e;

        public byte SpecFlag => 0x00;

        public byte StructDescrStartFlag => 0xab;

        public byte StructDescrEndFlag => 0xac;
    }
}
