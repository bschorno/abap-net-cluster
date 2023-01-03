using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class Int4Attribute : TypeAttribute
    {
        public Int4Attribute() { }

        internal override IType GetConverterType()
        {
            return new Int4Type();
        }
    }
}
