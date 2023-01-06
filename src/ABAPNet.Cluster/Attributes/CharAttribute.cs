using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class CharAttribute : TypeAttribute
    {
        public int Length { get; set; }

        public CharAttribute(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            Length = length;
        }

        internal override IType GetConverterType()
        {
            return new CharType(Length);
        }
    }
}
