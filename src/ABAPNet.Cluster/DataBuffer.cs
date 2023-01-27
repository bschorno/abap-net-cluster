using ABAPNet.Cluster.Attributes;
using ABAPNet.Cluster.Converter.Descriptors;
using System.Reflection;

namespace ABAPNet.Cluster
{
    public class DataBuffer
    {
        public const byte DataBufferEndByte = 0x04;

        private readonly DataBufferConfiguration _configuration;

        internal DataBufferConfiguration Configuration => _configuration;

        public DataBuffer()
        {
            _configuration = new DataBufferConfiguration(CodePage.UTF16LE, Endian.LittleEndian);
        }

        public DataBuffer(DataBufferConfiguration configuration)
            : this()
        {
            _configuration = configuration;
        }

        public byte[] Export<T>(T data) where T : new()
        {
            ArgumentNullException.ThrowIfNull(data);

            using MemoryStream memoryStream = new MemoryStream();
            using DataBufferWriter writer = new DataBufferWriter(this, memoryStream);
;
            writer.WriteHeader();

            foreach (var segment in GetSegments(data))
            {
                writer.OpenSegment(segment);
                writer.WriteSegmentHeader();
                segment.Descriptor.WriteDescription(writer);

                object? segmentData = segment.PropertyInfo.GetValue(data);
                segment.Descriptor.WriteContent(writer, segmentData);

                writer.CloseSegment();
            }

            writer.WriteByte(DataBufferEndByte);

            memoryStream.Close();
            return memoryStream.ToArray();
        }

        public T? Import<T>(byte[] cluster) where T : new()
        {
            ArgumentNullException.ThrowIfNull(cluster);

            using MemoryStream memoryStream = new MemoryStream(cluster);
            using DataBufferReader reader = new DataBufferReader(this, memoryStream);

            object data = Activator.CreateInstance<T>() ?? throw new Exception($"Couldn't create instance of {typeof(T)}");
            var segments = GetSegments((T)data);

            if (!reader.ReadHeader())
                throw new Exception("Invalid cluster header");

            while (reader.PeekByte() != DataBufferEndByte)
            {
                var segmentHeader = reader.ReadSegmentHeader();
                var segment = segments.FirstOrDefault(_ => _.Name == segmentHeader.SegmentName);
                if (segment == null)
                    throw new Exception($"Cluster field {segmentHeader.SegmentName} not found");

                if (segment.Descriptor.Type.KindFlag != segmentHeader.KindFlag ||
                    segment.Descriptor.Type.TypeFlag != segmentHeader.TypeFlag ||
                    segment.Descriptor.Type.SpecFlag != segmentHeader.SpecFlag)
                    throw new Exception($"Cluster field type mismatch");

                segment.SegmentStartOffset = segmentHeader.SegmentStartOffset;
                segment.SegmentEndOffset = segmentHeader.SegmentEndOffset;

                reader.OpenSegment(segment);

                segment.Descriptor.ReadDescription(reader);

                object? segmentData = null;
                segment.Descriptor.ReadContent(reader, ref segmentData);
                segment.PropertyInfo.SetValue(data, segmentData);
                
                reader.CloseSegment();
            }    

            if (reader.ReadByte() != DataBufferEndByte)
                throw new Exception("Invalid cluster end byte");

            return (T?)data;
        }

        private IEnumerable<DataBufferSegment> GetSegments<T>(T data)
        {
            var clusterProperties = typeof(T).GetProperties().Where(_ => Attribute.IsDefined(_, typeof(ClusterFieldNameAttribute))).ToArray();
            var descriptors = new List<string>();
            foreach (var clusterProperty in clusterProperties)
            {
                var clusterPropertyNameAttribute = clusterProperty.GetCustomAttribute<ClusterFieldNameAttribute>()!;
                var clusterPropertyTypeAttributes = clusterProperty.GetCustomAttributes<TypeAttribute>().ToArray();

                if (clusterPropertyTypeAttributes.Length == 0)
                    throw new Exception("No type attribute found on the property");
                if (clusterPropertyTypeAttributes.Length > 1)
                    throw new Exception("Property can only have one type attribute");

                if (descriptors.Contains(clusterPropertyNameAttribute.Name))
                    throw new Exception($"Multiple cluster fields with same name: {clusterPropertyNameAttribute.Name}");
                else
                    descriptors.Add(clusterPropertyNameAttribute.Name);

                var descriptor = Descriptor.Describe(clusterProperty.PropertyType, clusterPropertyTypeAttributes[0].GetConverterType());

                yield return new DataBufferSegment(clusterPropertyNameAttribute.Name, descriptor, clusterProperty);
            }
        }
    }
}
