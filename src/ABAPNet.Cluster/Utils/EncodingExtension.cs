using System.Text;

namespace ABAPNet.Cluster.Utils
{
    internal static class EncodingExtension
    {
        public static bool GetBytes(this Encoding encoding, char character, Span<byte> bytes)
        {
            if (bytes.Length < 2)
                return false;

            Span<char> chars = stackalloc char[1];
            chars[0] = character;

            return encoding.GetBytes(chars, bytes) == 1;
        }
    }
}
