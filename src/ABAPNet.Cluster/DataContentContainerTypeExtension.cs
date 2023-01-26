namespace ABAPNet.Cluster
{
    internal static class DataContentContainerTypeExtension
    {
        public static (byte Start, byte End) GetFlags(this DataContentContainerType dataContentContainerType)
            => dataContentContainerType switch
            {
                DataContentContainerType.FlatType => (0xbc, 0xbd),
                DataContentContainerType.StringType => (0xca, 0xcb),
                DataContentContainerType.TableType => (0xbe, 0xbf),
                _ => throw new Exception("Invalid enumerator")
            };

        public static DataContentContainerType GetFromStartFlag(byte startFlag)
            => startFlag switch
            {
                0xbc => DataContentContainerType.FlatType,
                0xca => DataContentContainerType.StringType,
                0xbe => DataContentContainerType.TableType,
                _ => throw new Exception("Invalid start flag")
            };

        public static DataContentContainerType GetFromEndFlag(byte endFlag)
            => endFlag switch
            {
                0xbd => DataContentContainerType.FlatType,
                0xcb => DataContentContainerType.StringType,
                0xbf => DataContentContainerType.TableType,
                _ => throw new Exception("Invalid end flag")
            };
    }
}
