using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class Int2TableAttribute : TypeAttribute
    {
        public Int2TableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType<Int2Type>(new Int2Type());
        }
    }
}
