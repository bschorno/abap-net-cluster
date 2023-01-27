﻿using ABAPNet.Cluster.Attributes;

namespace ABAPNet.Cluster.Test.Unit
{
    public class Write_DeepStruct
    {
        [Fact]
        public void Write_Deep_Structure()
        {
            DataBuffer dataBuffer = new DataBuffer();

            byte[] bufferExpected = {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x05, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x18, 0x00,
                0x00, 0x00, 0x95, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x53, 0x00, 0x54, 0x00, 0x52, 0x00, 0x55, 0x00, 0x43, 0x00, 0x54, 0x00,
                0xAB, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x18, 0xAA, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x08, 0xAA, 0x13, 0x00, 0x00, 0x00, 0x00, 0x08, 0xAB, 0x0E, 0x00,
                0x00, 0x00, 0x00, 0x08, 0xAA, 0x04, 0x00, 0x00, 0x00, 0x00, 0x01, 0xAF,
                0x04, 0x00, 0x00, 0x00, 0x00, 0x03, 0xAA, 0x08, 0x00, 0x00, 0x00, 0x00,
                0x04, 0xAC, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x08, 0xAC, 0x0F, 0x00, 0x00,
                0x00, 0x00, 0x18, 0xBC, 0x00, 0x00, 0x00, 0x08, 0x41, 0x00, 0x41, 0x00,
                0x41, 0x00, 0x41, 0x00, 0xBD, 0xCA, 0x00, 0x00, 0x00, 0x08, 0x42, 0x00,
                0x42, 0x00, 0x42, 0x00, 0x42, 0x00, 0xCB, 0xBC, 0x00, 0x00, 0x00, 0x08,
                0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xBD, 0x04
            };

            var bufferActual = dataBuffer.Export(new ClusterWithDeepStruct()
            {
                Field = new DeepStruct()
                {
                    Field1 = "AAAA",
                    Field2 = "BBBB",
                    Field3 = new DeeperStruct()
                    {
                        Field1 = 0x01,
                        Field2 = 1
                    }
                }
            });

            Assert.Equal(bufferExpected, bufferActual);
        }

        private struct ClusterWithDeepStruct
        {
            [ClusterFieldName("STRUCT")]
            [DeepStruct]
            public DeepStruct Field { get; set; }
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