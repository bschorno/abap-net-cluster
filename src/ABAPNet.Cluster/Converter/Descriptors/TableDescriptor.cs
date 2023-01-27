using ABAPNet.Cluster.Converter.Types;
using System.Collections;

namespace ABAPNet.Cluster.Converter.Descriptors
{
    internal class TableDescriptor : Descriptor<ITableType>
    {
        public TableDescriptor(ITableType tableType, Type declaringType, Descriptor? parentDescriptor) 
            : base(tableType, declaringType, parentDescriptor)
        {
            if (!declaringType.IsArray)
                throw new Exception("Type is not an array");

            Type? lineType = declaringType.GetElementType();
            if (lineType == null)
                throw new Exception("Type is not an array");

            _componentDescriptors = new Descriptor[1]
            {
                Describe(lineType, _type.GetLineType(), this)
            };
        }

        internal override int GetDescrByteLength()
        {
            return _componentDescriptors[0].GetDescrByteLength();
        }

        internal override void WriteDescription(DataBufferWriter writer)
        {
            writer.WriteByte(_type.StructDescrStartFlag);
            writer.WriteByte(_type.TypeFlag);
            writer.WriteByte(_type.SpecFlag);

            writer.WriteInvertedInt(GetDescrByteLength());

            _componentDescriptors[0].WriteDescription(writer);

            writer.WriteByte(_type.StructDescrEndFlag);
            writer.WriteByte(_type.TypeFlag);
            writer.WriteByte(_type.SpecFlag);

            writer.WriteInvertedInt(GetDescrByteLength());
        }

        internal override void WriteContent(DataBufferWriter writer, object? data)
        {
            if (data == null)
                throw new NullReferenceException("Data can't be null");

            if (data is not IList list)
                throw new InvalidTypeException(data, typeof(IList));

            writer.OpenDataContentContainer(DataContentContainerType.TableType);

            writer.WriteInvertedInt(GetDescrByteLength());
            writer.WriteInvertedInt(list.Count);

            foreach (var item in list)
            {
                _componentDescriptors[0].WriteContent(writer, item);

                DataContentContainerType? dataContentContainerType = writer.GetCurrentDataContentContainer();
                if (dataContentContainerType != null && 
                    (dataContentContainerType == DataContentContainerType.FlatType ||
                     dataContentContainerType == DataContentContainerType.StringType))
                    writer.CloseDataContentContainer(dataContentContainerType.Value);
            }

            writer.CloseDataContentContainer(DataContentContainerType.TableType);
        }

        internal override void ReadDescription(DataBufferReader reader)
        {
            if (_type.StructDescrStartFlag != reader.ReadByte() ||
                _type.TypeFlag != reader.ReadByte() ||
                _type.SpecFlag != reader.ReadByte() ||
                GetDescrByteLength() != reader.ReadInvertedInt())
                throw new Exception("Invalid type description");

            _componentDescriptors[0].ReadDescription(reader);

            if (_type.StructDescrEndFlag != reader.ReadByte() ||
                _type.TypeFlag != reader.ReadByte() ||
                _type.SpecFlag != reader.ReadByte() ||
                GetDescrByteLength() != reader.ReadInvertedInt())
                throw new Exception("Invalid type description");
        }

        internal override void ReadContent(DataBufferReader reader, ref object? data)
        {
            if (reader.GetOpeningDataContentContainer() != DataContentContainerType.TableType)
                throw new Exception("Invalid content");

            if (GetDescrByteLength() != reader.ReadInvertedInt())
                throw new Exception("Invalid content");

            var itemCount = reader.ReadInvertedInt();

            if (_declaringType.IsArray)
                data = Activator.CreateInstance(_declaringType, itemCount);
            else
                data = Activator.CreateInstance(_declaringType);
            if (data == null)
                throw new InvalidTypeException($"Couldn't create instance of type {_declaringType}");

            if (data is not IList list)
                throw new InvalidTypeException(data, typeof(IList));

            for (int i = 0; i < itemCount; i++) 
            {
                object? item = null;

                _componentDescriptors[0].ReadContent(reader, ref item);

                var dataContentContainer = reader.GetCurrentDataContentContainer();
                if (dataContentContainer != null &&
                    dataContentContainer == DataContentContainerType.FlatType ||
                    dataContentContainer == DataContentContainerType.StringType)
                    reader.GetClosingDataContentContainer();

                if (item != null)
                {
                    if (_declaringType.IsArray)
                        list[i] = item;
                    else
                        list.Add(item);
                }
            }

            if (reader.GetClosingDataContentContainer() != DataContentContainerType.TableType)
                throw new Exception("Invalid content");
        }
    }
}
