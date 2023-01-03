using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class FlatTableAttribute : TypeAttribute
    {
        public FlatTableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType();
        }
    }
}
