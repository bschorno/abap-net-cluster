using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class FlatStructAttribute : TypeAttribute
    {
        public FlatStructAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatStructType();
        }
    }
}
