using ABAPNet.Cluster.Attributes;
using ABAPNet.Cluster.Converter.Types;
using ABAPNet.Cluster.Utils;
using System.Reflection;

namespace ABAPNet.Cluster.Converter.Descriptors
{
    internal class StructDescriptor : Descriptor<IStructType>
    {
        private readonly Dictionary<Descriptor, PropertyInfo> _propertyInfos = new Dictionary<Descriptor, PropertyInfo>();

        public StructDescriptor(IStructType structType, Type declaringType, Descriptor? parentDescriptor)
            : base(structType, declaringType, parentDescriptor)
        {
            var properties = declaringType.GetProperties().Where(_ => Attribute.IsDefined(_, typeof(TypeAttribute))).ToArray();

            List<Descriptor> descriptors = new List<Descriptor>();
            int byteOffset = 0;

            for (int i = 0; i < properties.Length; i++)
            {
                var propertyTypeAttributes = properties[i].GetCustomAttributes<TypeAttribute>().ToArray();

                if (propertyTypeAttributes.Length == 0)
                    throw new Exception("No type attribute found on the property");
                if (propertyTypeAttributes.Length > 1)
                    throw new Exception("Property can only have one type attribute");

                Descriptor descriptor = Describe(properties[i].PropertyType, propertyTypeAttributes[0].GetConverterType(), this);

                OffsetDescriptor? offsetDescriptor = OffsetDescriptor.CreateIfNeeded(byteOffset, descriptor.GetDescrAlignmentFactor());
                if (offsetDescriptor != null)
                { 
                    descriptors.Add(offsetDescriptor);
                    byteOffset += offsetDescriptor.Offset;
                }

                if (descriptor.Type is IDefinedType definedType)
                    byteOffset += definedType.StructDescrByteLength;

                descriptors.Add(descriptor);
                _propertyInfos.Add(descriptor, properties[i]);
            }

            _componentDescriptors = descriptors.ToArray();

            OffsetDescriptor? trailingOffsetDescriptor = OffsetDescriptor.CreateIfNeeded(byteOffset, GetDescrAlignmentFactor());
            if (trailingOffsetDescriptor != null)
            {
                Array.Resize(ref _componentDescriptors, _componentDescriptors.Length + 1);
                _componentDescriptors[_componentDescriptors.Length - 1] = trailingOffsetDescriptor;
            }
        }

        internal override int GetDescrByteLength()
        {
            int byteLength = 0;
            bool containsTableDescr = false;
            foreach (var descriptor in _componentDescriptors)
            {
                if (descriptor.Type is IDefinedType definedType)
                    byteLength += definedType.StructDescrByteLength;
                else
                    byteLength += descriptor.GetDescrByteLength();

                if (descriptor.Type is ITableType)
                    containsTableDescr = true;
            }

            if (containsTableDescr && byteLength % 8 > 0)
                byteLength += 8 - byteLength % 8;

            return byteLength;
        }

        internal override void WriteDescription(DataBufferWriter writer)
        {
            if (_parentDescriptor is not TableDescriptor)
            {
                writer.WriteByte(_type.StructDescrStartFlag);
                writer.WriteByte(_type.TypeFlag);
                writer.WriteByte(0x00);

                writer.WriteInvertedInt(GetDescrByteLength());
            }

            foreach (var descriptor in _componentDescriptors)
            {
                descriptor.WriteDescription(writer);
            }

            if (_parentDescriptor is not TableDescriptor)
            {
                writer.WriteByte(_type.StructDescrEndFlag);
                writer.WriteByte(_type.TypeFlag);
                writer.WriteByte(0x00);

                writer.WriteInvertedInt(GetDescrByteLength());
            }
        }

        internal override void WriteContent(DataBufferWriter writer, object data)
        {
            if (data == null)
                throw new NullReferenceException("Data can't be null");

            foreach (var descriptor in _componentDescriptors)
            {
                if (descriptor is OffsetDescriptor)
                {
                    descriptor.WriteContent(writer, null);
                }
                else
                {
                    PropertyInfo propertyInfo = _propertyInfos[descriptor];
                    object? propertyValue = propertyInfo.GetValue(data);

                    if (propertyValue == null)
                        throw new NullReferenceException("Data can't be null");

                    descriptor.WriteContent(writer, propertyValue);
                }
            }
        }
    }
}
