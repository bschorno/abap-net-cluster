using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class TimsAttribute : TypeAttribute
    {
        public TimsAttribute() { }

        internal override IType GetConverterType()
        {
            return new TimsType();
        }
    }
}
