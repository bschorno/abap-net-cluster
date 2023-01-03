using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public class NumcAttribute : TypeAttribute
    {
        public int Length { get; set; }

        public NumcAttribute(int length)
        {
            Length = length;
        }

        internal override IType GetConverterType()
        {
            return new NumcType(Length);
        }
    }
}
