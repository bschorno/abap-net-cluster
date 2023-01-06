using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class StringAttribute : TypeAttribute
    {
        public StringAttribute() { }

        internal override IType GetConverterType()
        {
            return new StringType();
        }
    }
}
