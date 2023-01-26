using ABAPNet.Cluster.Attributes;
using ABAPNet.Cluster.Converter.Types;
using System.Runtime.CompilerServices;

namespace ABAPNet.Cluster.Converter.Descriptors
{
    internal abstract class Descriptor
    {
        protected Descriptor? _parentDescriptor;
        protected Descriptor[] _componentDescriptors;

        public abstract IType Type { get; }

        protected Descriptor(Descriptor? parentDescriptor)
        {
            _parentDescriptor = parentDescriptor;
        }

        internal abstract int GetDescrByteLength();

        internal abstract int GetDescrAlignmentFactor();

        internal abstract void WriteDescription(DataBufferWriter writer);

        internal abstract void WriteContent(DataBufferWriter writer, object? data);

        internal abstract void ReadDescription(DataBufferReader reader);

        internal abstract void ReadContent(DataBufferReader reader, ref object? data);

        internal static Descriptor Describe(Type type, IType converterType)
            => Describe(type, converterType, null);

        protected static Descriptor Describe(Type type, IType converterType, Descriptor? parentDescriptor)
        {
            Descriptor descriptor = converterType switch
            {
                ISimpleType simpleType => new SimpleDescriptor(simpleType, type, parentDescriptor),
                IStructType structType => new StructDescriptor(structType, type, parentDescriptor),
                ITableType tableType => new TableDescriptor(tableType, type, parentDescriptor),
                _ => throw new Exception("Invalid type attribute")
            };

            return descriptor;
        }
    }

    internal abstract class Descriptor<TType> : Descriptor where TType : IType
    {
        protected readonly Type _declaringType;
        protected readonly TType _type;
        private int? _alignmentFactor;

        public override IType Type => _type;

        public Type DeclaringType => _declaringType;

        protected Descriptor(TType type, Type declaringType, Descriptor? parentDescriptor)
            : base(parentDescriptor)
        {
            _declaringType = declaringType;
            _type = type;
        }

        internal override int GetDescrAlignmentFactor()
        {
            if (_alignmentFactor != null)
                return _alignmentFactor.Value;

            if (_type is IAlignedType alignedType)
                _alignmentFactor = alignedType.AlignmentFactor;
            else
                _alignmentFactor = _componentDescriptors.Select(_ => _.GetDescrAlignmentFactor()).Max();

            return _alignmentFactor.Value;
        }
    }
}
