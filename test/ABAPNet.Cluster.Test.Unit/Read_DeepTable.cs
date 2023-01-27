﻿using ABAPNet.Cluster.Attributes;

namespace ABAPNet.Cluster.Test.Unit
{
    public class Read_DeepTable
    {
        [Fact]
        public void Write_Deep_Table()
        {
            DataBuffer dataBuffer = new DataBuffer();

            var structExpected = new ClusterWithDeepTable()
            {
                Field = new DeepStruct[2]
                {
                    new DeepStruct()
                    {
                        Field1 = "AAAA",
                        Field2 = "BBBB",
                        Field3 = new DeeperStruct()
                        {
                            Field1 = 0x01,
                            Field2 = 1
                        }
                    },
                    new DeepStruct()
                    {
                        Field1 = "CCCC",
                        Field2 = "DDDD",
                        Field3 = new DeeperStruct()
                        {
                            Field1 = 0x02,
                            Field2 = 2
                        }
                    }
                }
            };

            var structActual = dataBuffer.Import<ClusterWithDeepTable>(new byte[] {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x06, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x18, 0x00,
                0x00, 0x00, 0xC7, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x54, 0x00, 0x41, 0x00, 0x42, 0x00, 0x4C, 0x00, 0x45, 0x00, 0xAD, 0x0F,
                0x00, 0x00, 0x00, 0x00, 0x18, 0xAA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08,
                0xAA, 0x13, 0x00, 0x00, 0x00, 0x00, 0x08, 0xAB, 0x0E, 0x00, 0x00, 0x00,
                0x00, 0x08, 0xAA, 0x04, 0x00, 0x00, 0x00, 0x00, 0x01, 0xAF, 0x04, 0x00,
                0x00, 0x00, 0x00, 0x03, 0xAA, 0x08, 0x00, 0x00, 0x00, 0x00, 0x04, 0xAC,
                0x0E, 0x00, 0x00, 0x00, 0x00, 0x08, 0xAE, 0x0F, 0x00, 0x00, 0x00, 0x00,
                0x18, 0xBE, 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x02, 0xBC, 0x00,
                0x00, 0x00, 0x08, 0x41, 0x00, 0x41, 0x00, 0x41, 0x00, 0x41, 0x00, 0xBD,
                0xCA, 0x00, 0x00, 0x00, 0x08, 0x42, 0x00, 0x42, 0x00, 0x42, 0x00, 0x42,
                0x00, 0xCB, 0xBC, 0x00, 0x00, 0x00, 0x08, 0x01, 0x00, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0xBD, 0xBC, 0x00, 0x00, 0x00, 0x08, 0x43, 0x00, 0x43,
                0x00, 0x43, 0x00, 0x43, 0x00, 0xBD, 0xCA, 0x00, 0x00, 0x00, 0x08, 0x44,
                0x00, 0x44, 0x00, 0x44, 0x00, 0x44, 0x00, 0xCB, 0xBC, 0x00, 0x00, 0x00,
                0x08, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0xBD, 0xBF, 0x04
            });

            Assert.Equal(structExpected.Field[0].Field1, structActual.Field[0].Field1);
            Assert.Equal(structExpected.Field[0].Field2, structActual.Field[0].Field2);
            Assert.Equal(structExpected.Field[0].Field3.Field1, structActual.Field[0].Field3.Field1);
            Assert.Equal(structExpected.Field[0].Field3.Field2, structActual.Field[0].Field3.Field2);
            Assert.Equal(structExpected.Field[1].Field1, structActual.Field[1].Field1);
            Assert.Equal(structExpected.Field[1].Field2, structActual.Field[1].Field2);
            Assert.Equal(structExpected.Field[1].Field3.Field1, structActual.Field[1].Field3.Field1);
            Assert.Equal(structExpected.Field[1].Field3.Field2, structActual.Field[1].Field3.Field2);
        }

        private struct ClusterWithDeepTable
        {
            [ClusterFieldName("TABLE")]
            [DeepTable]
            public DeepStruct[] Field { get; set; }
        }

        private struct DeepStruct
        {
            [Char(4)]
            public string Field1 { get; set; }

            [String]
            public string Field2 { get; set; }

            [FlatStruct]
            public DeeperStruct Field3 { get; set; }
        }

        private struct DeeperStruct
        {
            [Raw(1)]
            public byte Field1 { get; set; }

            [Int4]
            public int Field2 { get; set; }
        }
    }
}