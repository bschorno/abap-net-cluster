using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class DatsTableAttribute : TypeAttribute
    {
        public DatsTableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType<DatsType>(new DatsType());
        }
    }
}
