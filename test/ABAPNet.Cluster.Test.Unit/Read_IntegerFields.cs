﻿using ABAPNet.Cluster.Attributes;

namespace ABAPNet.Cluster.Test.Unit
{
    public class Read_IntegerFields
    {
        [Fact]
        public void Read_Int1()
        {
            DataBuffer dataBuffer = new DataBuffer();

            var structExpected = new ClusterWithInt1();

            var structActual = dataBuffer.Import<ClusterWithInt1>(new byte[] {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00,
                0x00, 0x00, 0x31, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x46, 0x00, 0x49, 0x00, 0x45, 0x00, 0x4C, 0x00, 0x44, 0x00, 0xBC, 0x00,
                0x00, 0x00, 0x01, 0x00, 0xBD, 0x04
            });

            Assert.Equal(structExpected.Field, structActual.Field);
        }

        [Fact]
        public void Read_Int2()
        {
            DataBuffer dataBuffer = new DataBuffer();

            var structExpected = new ClusterWithInt1();

            var structActual = dataBuffer.Import<ClusterWithInt2>(new byte[] {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x09, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00,
                0x00, 0x00, 0x32, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x46, 0x00, 0x49, 0x00, 0x45, 0x00, 0x4C, 0x00, 0x44, 0x00, 0xBC, 0x00,
                0x00, 0x00, 0x02, 0x00, 0x00, 0xBD, 0x04
            });

            Assert.Equal(structExpected.Field, structActual.Field);
        }

        [Fact]
        public void Read_Int4()
        {
            DataBuffer dataBuffer = new DataBuffer();

            var structExpected = new ClusterWithInt4();

            var structActual = dataBuffer.Import<ClusterWithInt4>(new byte[] {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x08, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00,
                0x00, 0x00, 0x34, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x46, 0x00, 0x49, 0x00, 0x45, 0x00, 0x4C, 0x00, 0x44, 0x00, 0xBC, 0x00,
                0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0xBD, 0x04
            });

            Assert.Equal(structExpected.Field, structActual.Field);
        }

        [Fact]
        public void Read_Int8()
        {
            DataBuffer dataBuffer = new DataBuffer();

            var structExpected = new ClusterWithInt8();

            var structActual = dataBuffer.Import<ClusterWithInt8>(new byte[] {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x1B, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00,
                0x00, 0x00, 0x38, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x46, 0x00, 0x49, 0x00, 0x45, 0x00, 0x4C, 0x00, 0x44, 0x00, 0xBC, 0x00,
                0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xBD,
                0x04
            });

            Assert.Equal(structExpected.Field, structActual.Field);
        }

        private class ClusterWithInt1
        {
            [ClusterFieldName("FIELD")]
            [Int1]
            public byte Field { get; set; }
        }

        private class ClusterWithInt2
        {
            [ClusterFieldName("FIELD")]
            [Int2]
            public short Field { get; set; }
        }

        private class ClusterWithInt4
        {
            [ClusterFieldName("FIELD")]
            [Int4]
            public int Field { get; set; }
        }

        private class ClusterWithInt8
        {
            [ClusterFieldName("FIELD")]
            [Int8]
            public long Field { get; set; }
        }
    }
}
