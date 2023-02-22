using ABAPNet.Cluster.Attributes;
using ABAPNet.Cluster.Converter.Types;
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
                else // <- Inserted later (Must be kept in mind)
                    byteOffset += descriptor.GetDescrByteLength();

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
                writer.WriteByte(_type.SpecFlag);

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
                writer.WriteByte(_type.SpecFlag);

                writer.WriteInvertedInt(GetDescrByteLength());
            }
        }

        internal override void WriteContent(DataBufferWriter writer, object? data)
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

                    descriptor.WriteContent(writer, propertyValue);
                }
            }
        }

        internal override void ReadDescription(DataBufferReader reader)
        {
            if (_parentDescriptor is not TableDescriptor)
            {
                if (_type.StructDescrStartFlag != reader.ReadByte() ||
                    _type.TypeFlag != reader.ReadByte() ||
                    _type.SpecFlag != reader.ReadByte() ||
                    GetDescrByteLength() != reader.ReadInvertedInt())
                    throw new Exception("Invalid type description");
            }

            foreach (var descriptor in _componentDescriptors)
            {
                descriptor.ReadDescription(reader);
            }

            if (_parentDescriptor is not TableDescriptor)
            {
                if (_type.StructDescrEndFlag != reader.ReadByte() ||
                    _type.TypeFlag != reader.ReadByte() ||
                    _type.SpecFlag != reader.ReadByte() ||
                    GetDescrByteLength() != reader.ReadInvertedInt())
                    throw new Exception("Invalid type description");
            }
        }

        internal override void ReadContent(DataBufferReader reader, ref object? data)
        {
            data = Activator.CreateInstance(_declaringType);
            if (data == null)
                throw new InvalidTypeException($"Couldn't create instance of type {_declaringType}");

            foreach (var descriptor in _componentDescriptors)
            {
                if (descriptor is OffsetDescriptor)
                {
                    object? dummy = null;
                    descriptor.ReadContent(reader, ref dummy);
                }
                else
                {
                    PropertyInfo propertyInfo = _propertyInfos[descriptor];
                    object? propertyValue = null;
                    descriptor.ReadContent(reader, ref propertyValue);
                    propertyInfo.SetValue(data, propertyValue);
                }
            }
        }
    }
}
