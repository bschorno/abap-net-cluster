using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class DatsAttribute : TypeAttribute
    {
        public DatsAttribute() { }

        internal override IType GetConverterType()
        {
            return new DatsType();
        }
    }
}
