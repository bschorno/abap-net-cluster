using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class DeepStructAttribute : TypeAttribute
    {
        public DeepStructAttribute() { }

        internal override IType GetConverterType()
        {
            return new DeepStructType();
        }
    }
}
