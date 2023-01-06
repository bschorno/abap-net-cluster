using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class NumcAttribute : TypeAttribute
    {
        public int Length { get; set; }

        public NumcAttribute(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            Length = length;
        }

        internal override IType GetConverterType()
        {
            return new NumcType(Length);
        }
    }
}
