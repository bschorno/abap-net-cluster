using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class Int8Attribute : TypeAttribute
    {
        public Int8Attribute() { }

        internal override IType GetConverterType()
        {
            return new Int8Type();
        }
    }
}
