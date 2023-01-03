using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class CharAttribute : TypeAttribute
    {
        public int Length { get; set; }

        public CharAttribute(int length)
        {
            Length = length;
        }

        internal override IType GetConverterType()
        {
            return new CharType(Length);
        }
    }
}
