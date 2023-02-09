using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{
    public sealed class StringTableAttribute : TypeAttribute
    {
        public StringTableAttribute() { }

        internal override IType GetConverterType()
        {
            return new DeepTableType<StringType>(new StringType());
        }
    }
}
