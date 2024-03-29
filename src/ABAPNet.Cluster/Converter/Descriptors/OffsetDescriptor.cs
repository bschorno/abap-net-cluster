﻿using ABAPNet.Cluster.Converter.Types;

namespace ABAPNet.Cluster.Converter.Descriptors
{
    internal class OffsetDescriptor : Descriptor
    {
        struct OffsetType : IType
        {
            public byte KindFlag => 0xaf;

            public byte TypeFlag => 0x04;

            public byte SpecFlag => 0x00;
        }

        private static OffsetType StaticOffsetType = new OffsetType();

        private readonly int _offset;

        public override IType Type => StaticOffsetType;

        public int Offset => _offset;

        public OffsetDescriptor(int offset) 
            : base(null)
        {
            _offset = offset;
        }

        public static OffsetDescriptor? CreateIfNeeded(int byteOffset, int alignmentFactor)
        {
            int offset = alignmentFactor - byteOffset % alignmentFactor;
            if (offset > 0 && offset < alignmentFactor)
                return new OffsetDescriptor(offset);
            return null;
        }

        internal override int GetDescrAlignmentFactor() => 0;

        internal override int GetDescrByteLength() => _offset;

        internal override void WriteDescription(DataBufferWriter writer)
        {
            writer.WriteByte(Type.KindFlag);
            writer.WriteByte(Type.TypeFlag);
            writer.WriteByte(Type.SpecFlag);

            writer.WriteInvertedInt(_offset);
        }

        internal override void WriteContent(DataBufferWriter writer, object? data)
        {
            writer.OpenDataContentContainer(DataContentContainerType.FlatType);

            writer.Write(new byte[_offset]);
        }

        internal override void ReadDescription(DataBufferReader reader)
        {
            if (Type.KindFlag != reader.ReadByte() ||
                Type.TypeFlag != reader.ReadByte() ||
                Type.SpecFlag != reader.ReadByte() ||
                _offset != reader.ReadInvertedInt())
                throw new Exception("Invalid type description");
        }

        internal override void ReadContent(DataBufferReader reader, ref object? data)
        {
            if (reader.GetOpeningDataContentContainer() != DataContentContainerType.FlatType)
                throw new Exception("Invalid content");

            reader.Skip(_offset);
        }
    }
}
