using ABAPNet.Cluster.Converter.Types;
using ABAPNet.Cluster.Utils;
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
            int byteLength = _componentDescriptors[0].GetDescrByteLength();
            //int alignmentFactor = GetDescrAlignmentFactor();

            //if (alignmentFactor > 0 && byteLength % alignmentFactor > 0)
            //    byteLength += alignmentFactor - byteLength % alignmentFactor;

            return byteLength;
        }

        internal override void WriteDescription(DataBufferWriter writer)
        {
            writer.WriteByte(_type.StructDescrStartFlag);
            writer.WriteByte(_type.TypeFlag);
            writer.WriteByte(0x00);

            writer.WriteInvertedInt(GetDescrByteLength());

            _componentDescriptors[0].WriteDescription(writer);

            writer.WriteByte(_type.StructDescrEndFlag);
            writer.WriteByte(_type.TypeFlag);
            writer.WriteByte(0x00);

            writer.WriteInvertedInt(GetDescrByteLength());
        }

        internal override void WriteContent(DataBufferWriter writer, object data)
        {
            if (data == null)
                throw new NullReferenceException("Data can't be null");

            if (data is not IList list)
                throw new Exception("Data is not an array");

            writer.CurrentSegment.OpenDataContentContainer(DataBufferSegment.DataContentContainerType.TableType);

            writer.WriteInvertedInt(GetDescrByteLength());
            writer.WriteInvertedInt(list.Count);

            foreach (var item in list)
            {
                _componentDescriptors[0].WriteContent(writer, item);

                DataBufferSegment.DataContentContainerType? dataContentContainerType = writer.CurrentSegment.GetCurrentDataContentContainer();
                if (dataContentContainerType != null && 
                    (dataContentContainerType == DataBufferSegment.DataContentContainerType.FlatType ||
                     dataContentContainerType == DataBufferSegment.DataContentContainerType.StringType))
                    writer.CurrentSegment.CloseDataContenetContainer(dataContentContainerType.Value);
            }

            writer.CurrentSegment.CloseDataContenetContainer(DataBufferSegment.DataContentContainerType.TableType);
        }
    }
}
