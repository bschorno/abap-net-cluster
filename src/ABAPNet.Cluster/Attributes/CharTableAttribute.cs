using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class CharTableAttribute : TypeAttribute
    {
        public int Length { get; set; }

        public CharTableAttribute(int length)
        {
            Length = length;
        }

        internal override IType GetConverterType()
        {
            return new CharTableType(Length);
        }
    }
}
