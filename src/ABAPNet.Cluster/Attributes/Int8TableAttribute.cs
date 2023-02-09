using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class Int8TableAttribute : TypeAttribute
    {
        public Int8TableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType<Int8Type>(new Int8Type());
        }
    }
}
