using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class FlatStructAttribute : TypeAttribute
    {
        public FlatStructAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatStructType();
        }
    }
}
