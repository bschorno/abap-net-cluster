using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class Int1TableAttribute : TypeAttribute
    {
        public Int1TableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType<Int1Type>(new Int1Type());
        }
    }
}
