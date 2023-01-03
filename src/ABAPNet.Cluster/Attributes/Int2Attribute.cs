using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class Int2Attribute : TypeAttribute
    {
        public Int2Attribute() { }

        internal override IType GetConverterType()
        {
            return new Int2Type();
        }
    }
}
