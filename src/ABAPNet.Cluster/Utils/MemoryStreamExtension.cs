namespace ABAPNet.Cluster.Utils
{
    internal static class MemoryStreamExtension
    {
        public static void WriteInvertedInt(this MemoryStream memoryStream, int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);

            memoryStream.Write(buffer);
        }
    }
}
