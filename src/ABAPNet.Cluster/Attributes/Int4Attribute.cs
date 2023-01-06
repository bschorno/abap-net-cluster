using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class Int4Attribute : TypeAttribute
    {
        public Int4Attribute() { }

        internal override IType GetConverterType()
        {
            return new Int4Type();
        }
    }
}
