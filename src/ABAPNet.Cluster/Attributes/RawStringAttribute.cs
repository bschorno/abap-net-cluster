using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class RawStringAttribute : TypeAttribute
    {
        public RawStringAttribute() { }

        internal override IType GetConverterType()
        {
            return new RawStringType();
        }
    }
}
