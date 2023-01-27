using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Converter.Descriptors
{
    internal class SimpleDescriptor : Descriptor<ISimpleType>
    {
        public SimpleDescriptor(ISimpleType simpleType, Type declaringType, Descriptor? parentDescriptor)
            : base(simpleType, declaringType, parentDescriptor) 
        {
        
        }

        internal override int GetDescrByteLength()
        {
            return _type.StructDescrByteLength;
        }

        internal override void WriteDescription(DataBufferWriter writer)
        {
            if (_parentDescriptor != null)
            {
                writer.WriteByte(_type.StructDescrFlag);
                writer.WriteByte(_type.TypeFlag);
                writer.WriteByte(_type.SpecFlag);

                writer.WriteInvertedInt(GetDescrByteLength());
            }
        }

        internal override void WriteContent(DataBufferWriter writer, object? data)
        {
            writer.OpenDataContentContainer(_type is IFlatType ? DataContentContainerType.FlatType : DataContentContainerType.StringType);

            if (_type is IFlatType)
            {
                writer.Write(_type.GetBytes(data, writer));
            }
            else if (_type is IStringType)
            {
                ReadOnlySpan<byte> buffer = _type.GetBytes(data, writer);
                writer.WriteInvertedInt(buffer.Length);
                writer.Write(buffer);
            }
        }

        internal override void ReadDescription(DataBufferReader reader)
        {
            if (_parentDescriptor != null)
            {
                if (_type.StructDescrFlag != reader.ReadByte() ||
                    _type.TypeFlag != reader.ReadByte() ||
                    _type.SpecFlag != reader.ReadByte() ||
                    GetDescrByteLength() != reader.ReadInvertedInt())
                    throw new Exception("Invalid type description");
            }
        }

        internal override void ReadContent(DataBufferReader reader, ref object? data)
        {
            if (_declaringType == typeof(string))
                data = string.Empty;
            else
                data = Activator.CreateInstance(_declaringType);
            if (data == null)
                throw new InvalidTypeException($"Couldn't create instance of type {_declaringType}");

            var dataContentContainer = reader.GetOpeningDataContentContainer();

            if (_type is IFlatType)
            {
                if (dataContentContainer != DataContentContainerType.FlatType)
                {
                    reader.GetClosingDataContentContainer();
                    dataContentContainer = reader.GetOpeningDataContentContainer();
                    if (dataContentContainer != DataContentContainerType.FlatType)
                        throw new Exception("Invalid content");
                }

                Span<byte> dataBuffer = stackalloc byte[GetDescrByteLength()];
                reader.Read(dataBuffer);

                _type.SetBytes(ref data, dataBuffer, reader);
            }
            else if (_type is IStringType)
            {
                if (dataContentContainer != DataContentContainerType.StringType)
                {
                    reader.GetClosingDataContentContainer();
                    dataContentContainer = reader.GetOpeningDataContentContainer();
                    if (dataContentContainer != DataContentContainerType.StringType)
                        throw new Exception("Invalid content");
                }

                var dataLength = reader.ReadInvertedInt();
                Span<byte> dataBuffer = new byte[dataLength];
                reader.Read(dataBuffer);

                _type.SetBytes(ref data, dataBuffer, reader);
            }
        }
    }
}
