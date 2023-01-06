using System.Numerics;

namespace ABAPNet.Cluster.Converter.Types
{
    internal class DecimalType : IFlatType, IType
    {
        private byte _length;
        private byte _decimals;

        public byte KindFlag => 0x01;

        public byte TypeFlag => 0x02;

        public byte SpecFlag => _decimals;

        public byte StructDescrFlag => 0xaa;

        public int StructDescrByteLength => _length;

        public int AlignmentFactor => 1;

        public int Length => _length;

        public int Decimals => _decimals;

        public DecimalType(byte length, byte decimals)
        {
            if (length <= 0 && length > 16) throw new ArgumentNullException("Length should be between 1 and 16", nameof(length));
            if (decimals < 0 && decimals > 14) throw new ArgumentNullException("Decimals should be between 0 and 14", nameof(decimals));
            _length = length;
            _decimals = decimals;
        }   

        public ReadOnlySpan<byte> GetBytes(object? data)
        {
            Span<byte> buffer = new Span<byte>(new byte[_length]);

            if (data == null)
                data = new decimal();

            if (data is not decimal decimalValue)
            {
                decimalValue = data switch
                {
                    float value => (decimal)value,
                    double value => (decimal)value,
                    _ => throw new InvalidTypeException(data, typeof(decimal), typeof(double), typeof(float))
                };
            }

            GetBytes(decimalValue, ref buffer);

            return buffer;
        }

        private void GetBytes(decimal value, ref Span<byte> buffer)
        {
            decimal tmpValue = Math.Round(value, Decimals);

            long upperValue = (long)Math.Truncate(tmpValue);
            long lowerValue = (long)(tmpValue % 1 * (decimal)Math.Pow(10, Decimals));

            Span<byte> upperBuffer = stackalloc byte[10];
            Span<byte> lowerBuffer = stackalloc byte[10];

            var tmp = upperValue;
            var index = 0;
            do
            {
                upperBuffer[index++] = (byte)(tmp % 100);
                tmp /= 100;
            } while (tmp > 0);

            tmp = lowerValue;
            index = 0;
            do
            {
                lowerBuffer[index++] = (byte)(tmp % 100);
                tmp /= 100;
            } while (tmp > 0);

            buffer.Clear();

            byte carryByte = upperValue > 0 ? (byte)0xc0 : (byte)0xd0;
            byte count = 0;
            byte upperIndex = 0;
            byte lowerIndex = 0;

            for (int i = Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte)(carryByte >> 4);
                carryByte = 0;

                byte bcd = 0x00;
                if (count < Decimals)
                {
                    if (lowerIndex >= 10)
                        continue;
                    bcd = (byte)((lowerBuffer[lowerIndex] / 10 * 16) + (lowerBuffer[lowerIndex] % 10));
                    lowerIndex++;
                }
                else
                {
                    if (Decimals % 2 == 1)
                    {
                        if (upperIndex >= 10)
                            continue;
                        buffer[i] = (byte)((upperBuffer[upperIndex] / 10 * 16) + (upperBuffer[upperIndex] % 10));
                        upperIndex++;
                        continue;
                    }

                    if (upperIndex >= 10)
                        continue;
                    bcd = (byte)((upperBuffer[upperIndex] / 10 * 16) + (upperBuffer[upperIndex] % 10));
                    upperIndex++;
                }
                count += 2;

                buffer[i] = (byte)(buffer[i] | (byte)(bcd << 4));
                carryByte = bcd;
            }
        }
    }
}
