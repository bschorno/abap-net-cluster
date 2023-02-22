using ABAPNet.Cluster.Attributes;

namespace ABAPNet.Cluster.Test
{
    internal class TestHxD
    {
        public static Cluster GetCluster()
        {
            return new Cluster()
            {
                Field = new Structure()
                {
                    Table1 = new int[0]
                }
            };

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

        internal class Cluster
        {
            [ClusterFieldName("STRUCT")]
            [DeepStruct]
            public Structure Field { get; set; }
        }

        internal struct Structure
        {
            [Char(1)]
            public char Field1 { get; set; }

            [FlatStruct]
            public DateTimeStruct Struct1 { get; set; }

            [Int4Table]
            public int[] Table1 { get; set; }
        }

        internal struct DateTimeStruct
        {
            [Dats]
            public DateOnly Date { get; set; }

            [Tims]
            public TimeOnly Time { get; set; }
        }

        //internal struct MultipleClusterFields
        //{
        //    [ClusterFieldName("FIELD1")]
        //    [Int4]
        //    public int Field1 { get; set; }

        //    [ClusterFieldName("FIELD2")]
        //    [Int4]
        //    public int Field2 { get; set; }
        //}

        //internal struct Cluster
        //{
        //    [ClusterFieldName("STRUCT")]
        //    [DeepStruct]
        //    public Data STRUCT { get; set; }
        //}

        //internal struct Data
        //{
        //    [FlatTable]
        //    public NestedData[] NESTED { get; set; }
        //}

        //internal struct NestedData
        //{
        //    [Raw(1)]
        //    public byte FIELD1 { get; set; }

        //    [Char(1)]
        //    public string FIELD2 { get; set; }
        //}
    }
}
