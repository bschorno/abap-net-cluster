using ABAPNet.Cluster.Converter.Types;
using ABAPNet.Cluster.Utils;

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
                writer.WriteByte(0x00);

                writer.WriteInvertedInt(GetDescrByteLength());
            }
            //writer.WriteByte(_parentDescriptor == null ? _type.KindFlag : _type.StructDescrFlag);
            //writer.WriteByte(_type.TypeFlag);
            //writer.WriteByte(0x00);

            //writer.WriteInvertedInt(GetDescrByteLength());
        }

        internal override void WriteContent(DataBufferWriter writer, object data)
        {
            if (data == null)
                throw new NullReferenceException("Data can't be null");

            writer.CurrentSegment.OpenDataContentContainer(_type is IFlatType ? DataBufferSegment.DataContentContainerType.FlatType : DataBufferSegment.DataContentContainerType.StringType);

            if (_type is IFlatType)
            {
                writer.Write(_type.GetBytes(data));
            }
            else if (_type is IStringType)
            {
                ReadOnlySpan<byte> buffer = _type.GetBytes(data);
                writer.WriteInvertedInt(buffer.Length);
                writer.Write(buffer);
            }
        }
    }
}
