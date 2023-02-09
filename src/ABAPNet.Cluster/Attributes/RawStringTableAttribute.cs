using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class RawStringTableAttribute : TypeAttribute
    {
        public RawStringTableAttribute() { }

        internal override IType GetConverterType()
        {
            return new DeepTableType<RawStringType>(new RawStringType());
        }
    }
}
