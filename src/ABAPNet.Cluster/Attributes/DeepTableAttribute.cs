using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class DeepTableAttribute : TypeAttribute
    {
        public DeepTableAttribute() { }

        internal override IType GetConverterType()
        {
            return new DeepTableType();
        }
    }
}
