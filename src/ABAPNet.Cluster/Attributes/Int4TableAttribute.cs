using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class Int4TableAttribute : TypeAttribute
    {
        public Int4TableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType<Int4Type>(new Int4Type());
        }
    }
}
