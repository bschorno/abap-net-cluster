using ABAPNet.Cluster.Attributes;

namespace ABAPNet.Cluster.Test
{
    internal class TestHxD
    {
        public static MultipleClusterFields GetCluster()
        {
            return new MultipleClusterFields();

            //return new Cluster()
            //{
            //    STRUCT = new Data()
            //    {
            //        NESTED = new NestedData[]
            //        {
            //            new NestedData()
            //            {
            //                FIELD1 = 0,
            //                FIELD2 = string.Empty
            //            }
            //        }
            //    }
            //};
        }

        internal struct MultipleClusterFields
        {
            [ClusterFieldName("FIELD1")]
            [Int4]
            public int Field1 { get; set; }

            [ClusterFieldName("FIELD2")]
            [Int4]
            public int Field2 { get; set; }
        }

        internal struct Cluster
        {
            [ClusterFieldName("STRUCT")]
            [DeepStruct]
            public Data STRUCT { get; set; }
        }

        internal struct Data
        {
            [FlatTable]
            public NestedData[] NESTED { get; set; }
        }

        internal struct NestedData
        {
            [Raw(1)]
            public byte FIELD1 { get; set; }

            [Char(1)]
            public string FIELD2 { get; set; }
        }
    }
}
