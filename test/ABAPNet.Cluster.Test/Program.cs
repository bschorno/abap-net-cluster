using ABAPNet.Cluster;
using ABAPNet.Cluster.Attributes;
using ABAPNet.Cluster.Test;

DataBuffer dataBuffer = new DataBuffer();
byte[] byteData = dataBuffer.Export(TestHxD.GetCluster());

File.WriteAllBytes(@"C:\Users\Bruno Schorno\Desktop\data_buffer_net.bin", byteData);

ReadOnlySpan<char> hexString = Convert.ToHexString(byteData);
int offset = 0;
while (offset < hexString.Length)
{
    ReadOnlySpan<char> hexString16 = hexString.Slice(offset, Math.Min(32, hexString.Length - offset));
    Console.WriteLine(hexString16.ToString());
    offset += 32;
}