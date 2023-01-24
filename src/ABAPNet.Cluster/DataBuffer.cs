using ABAPNet.Cluster.Attributes;
using ABAPNet.Cluster.Converter.Descriptors;
using System.Reflection;
using System.Text;

namespace ABAPNet.Cluster
{
    public class DataBuffer
    {
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

        public byte[] Export<T>(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            MemoryStream memoryStream = new MemoryStream();
            DataBufferWriter writer = new DataBufferWriter(this, memoryStream);

            writer.Write(new byte[16] { 0xff, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33, 0x00, 0x00, 0x00, 0x00 }, 0, 16);

            var clusterProperties = typeof(T).GetProperties().Where(_ => Attribute.IsDefined(_, typeof(ClusterFieldNameAttribute))).ToArray();
            var descriptors = new Dictionary<string, Descriptor>();
            foreach (var clusterProperty in clusterProperties)
            {
                var clusterPropertyNameAttribute = clusterProperty.GetCustomAttribute<ClusterFieldNameAttribute>();
                var clusterPropertyTypeAttributes = clusterProperty.GetCustomAttributes<TypeAttribute>().ToArray();
                var clusterPropertyValue = clusterProperty.GetValue(data);

                if (clusterPropertyTypeAttributes.Length == 0)
                    throw new Exception("No type attribute found on the property");
                if (clusterPropertyTypeAttributes.Length > 1)
                    throw new Exception("Property can only have one type attribute");

                var descriptor = Descriptor.Describe(clusterProperty.PropertyType, clusterPropertyTypeAttributes[0].GetConverterType());

                if (!descriptors.TryAdd(clusterPropertyNameAttribute.Name, descriptor))
                    throw new Exception($"Multiple cluster fields with same name: {clusterPropertyNameAttribute.Name}");

                WriteClusterField(writer, clusterPropertyNameAttribute.Name, descriptor, clusterPropertyValue);
            }

            writer.Write(new byte[1] { 0x04 }, 0, 1);

            memoryStream.Close();
            return memoryStream.ToArray();
        }

        public T Import<T>(byte[] cluster)
        {
            throw new NotImplementedException();
        }

        private void WriteClusterField(DataBufferWriter writer, string name, Descriptor descriptor, object? data)
        {
            DataBufferSegment segment = writer.OpenSegment(name);

            writer.WriteByte(descriptor.Type.KindFlag);
            writer.WriteByte(descriptor.Type.TypeFlag);
            writer.WriteByte(descriptor.Type.SpecFlag);

            writer.WriteInvertedInt(descriptor.GetDescrByteLength());
            writer.Write(new byte[4]);

            writer.Write(BitConverter.GetBytes(name.Length), 0, 1);
            writer.Write(new byte[20]);
            writer.Write(Encoding.Unicode.GetBytes(name));

            descriptor.WriteDescription(writer);
            descriptor.WriteContent(writer, data);

            segment.Close();
        }
    }
}
