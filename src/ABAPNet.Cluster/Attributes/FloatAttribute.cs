using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class FloatAttribute : TypeAttribute
    {
        public FloatAttribute() { }

        internal override IType GetConverterType()
        {
            return new FloatType();
        }
    }
}
