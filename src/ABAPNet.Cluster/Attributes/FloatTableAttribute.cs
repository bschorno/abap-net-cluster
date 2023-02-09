using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class FloatTableAttribute : TypeAttribute
    {
        public FloatTableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType<FloatType>(new FloatType());
        }
    }
}
