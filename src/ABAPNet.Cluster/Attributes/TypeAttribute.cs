using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public abstract class TypeAttribute : Attribute
    {
        internal abstract IType GetConverterType();
    }
}
