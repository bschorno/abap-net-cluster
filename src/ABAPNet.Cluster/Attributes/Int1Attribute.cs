using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class Int1Attribute : TypeAttribute
    {
        public Int1Attribute() { }

        internal override IType GetConverterType()
        {
            return new Int1Type();
        }
    }
}
