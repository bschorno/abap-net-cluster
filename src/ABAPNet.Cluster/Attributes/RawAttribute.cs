using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class RawAttribute : TypeAttribute
    {
        public int Length { get; set; }

        public RawAttribute(int length)
        {
            Length = length;
        }

        internal override IType GetConverterType()
        {
            return new RawType(Length);
        }
    }
}
