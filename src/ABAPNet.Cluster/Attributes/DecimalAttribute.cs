using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class DecimalAttribute : TypeAttribute
    {
        public byte Length { get; set; }

        public byte Decimals { get; set; }

        public DecimalAttribute(byte length)
            : this(length, 0)
        { }

        public DecimalAttribute(byte length, byte decimals)
        {
            if (length <= 0 && length > 16) throw new ArgumentNullException("Length should be between 1 and 16", nameof(length));
            if (decimals < 0 && decimals > 14) throw new ArgumentNullException("Decimals should be between 0 and 14", nameof(decimals));
            Length = length;
            Decimals = decimals;
        }

        internal override IType GetConverterType()
        {
            return new DecimalType(Length, Decimals);
        }
    }
}
