using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class RawStringAttribute : TypeAttribute
    {
        public RawStringAttribute() { }

        internal override IType GetConverterType()
        {
            return new RawStringType();
        }
    }
}
