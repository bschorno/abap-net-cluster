using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class TimsTableAttribute : TypeAttribute
    {
        public TimsTableAttribute() { }

        internal override IType GetConverterType()
        {
            return new FlatTableType<TimsType>(new TimsType());
        }
    }
}
