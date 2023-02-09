using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class RawTableAttribute : TypeAttribute
    {
        public int Length { get; set; }

        public RawTableAttribute(int length)
        {
            if (length <= 0) throw new ArgumentException("Length should be greater than zero", nameof(length));
            Length = length;
        }

        internal override IType GetConverterType()
        {
            return new FlatTableType<RawType>(new RawType(Length));
        }
    }
}