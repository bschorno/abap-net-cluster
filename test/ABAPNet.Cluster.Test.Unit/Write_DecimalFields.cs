﻿using ABAPNet.Cluster.Attributes;

namespace ABAPNet.Cluster.Test.Unit
{
    public class Write_DecimalFields
    {
        [Fact]
        public void Write_Decimal_8_6()
        {
            DataBuffer dataBuffer = new DataBuffer();

            byte[] bufferExpected = {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x06, 0x00, 0x00, 0x00, 0x08, 0x00,
                0x00, 0x00, 0x38, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x46, 0x00, 0x49, 0x00, 0x45, 0x00, 0x4C, 0x00, 0x44, 0x00, 0xBC, 0x00,
                0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x11, 0x16, 0x66, 0x00, 0x0C, 0xBD,
                0x04
            };

            var bufferActual = dataBuffer.Export(new ClusterWithDecimal_8_6() { Field = 111.666 });

            Assert.Equal(bufferExpected, bufferActual);
        }

        [Fact]
        public void Write_Decimal_9_5()
        {
            DataBuffer dataBuffer = new DataBuffer();

            byte[] bufferExpected = {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x05, 0x00, 0x00, 0x00, 0x09, 0x00,
                0x00, 0x00, 0x39, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x46, 0x00, 0x49, 0x00, 0x45, 0x00, 0x4C, 0x00, 0x44, 0x00, 0xBC, 0x00,
                0x00, 0x00, 0x09, 0x00, 0x01, 0x11, 0x11, 0x11, 0x11, 0x66, 0x66, 0x6C,
                0xBD, 0x04
            };


            var bufferActual = dataBuffer.Export(new ClusterWithDecimal_9_5() { Field = 111111111.66666 });

            Assert.Equal(bufferExpected, bufferActual);
        }

        [Fact]
        public void Write_Decimal_16_14()
        {
            DataBuffer dataBuffer = new DataBuffer();

            byte[] bufferExpected = {
                0xFF, 0x06, 0x02, 0x01, 0x01, 0x02, 0x80, 0x00, 0x34, 0x31, 0x30, 0x33,
                0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x0E, 0x00, 0x00, 0x00, 0x10, 0x00,
                0x00, 0x00, 0x40, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x46, 0x00, 0x49, 0x00, 0x45, 0x00, 0x4C, 0x00, 0x44, 0x00, 0xBC, 0x00,
                0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x11, 0x11, 0x11, 0x11, 0x16,
                0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x6C, 0xBD, 0x04
            };


            var bufferActual = dataBuffer.Export(new ClusterWithDecimal_16_14() { Field = 111111111.66666666666666m });

            Assert.Equal(bufferExpected, bufferActual);
        }


        private class ClusterWithDecimal_8_6
        {
            [ClusterFieldName("FIELD")]
            [Decimal(8, 6)]
            public double Field { get; set; }
        }

        private class ClusterWithDecimal_9_5
        {
            [ClusterFieldName("FIELD")]
            [Decimal(9, 5)]
            public double Field { get; set; }
        }

        private class ClusterWithDecimal_16_14
        {
            [ClusterFieldName("FIELD")]
            [Decimal(16, 14)]
            public decimal Field { get; set; }
        }
    }
}
