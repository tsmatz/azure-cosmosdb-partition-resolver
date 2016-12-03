using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace DocumentDbPartitionResolveSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string keyValue = "div200";
            uint hash;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)8);
                    writer.Write(Encoding.UTF8.GetBytes(keyValue));
                    writer.Write((byte)0);
                    hash = MurmurHash(stream.GetBuffer(), stream.Length, 0);
                }
            }

            Console.WriteLine("The partition key of {0} is {1}.",
                keyValue,
                ToHexEncodedBinaryString(hash));  // 05C1EDBFC1A70A
            Console.ReadLine();
        }

        //
        // Helper for Murmur Hash
        //

        static uint MurmurHash(byte[] bytes, long length, uint seed = 0)
        {
            uint num = 0xcc9e2d51;
            uint num2 = 0x1b873593;
            uint n = seed;
            for (int i = 0; i < (length - 3L); i += 4)
            {
                uint num5 = BitConverter.ToUInt32(bytes, i) * num;
                num5 = RotateLeft(num5, 15) * num2;
                n ^= num5;
                n = (RotateLeft(n, 13) * 5) + 0xe6546b64;
            }
            uint num6 = 0;
            long num7 = length & 3L;
            if ((num7 <= 3L) && (num7 >= 1L))
            {
                switch (((int)(num7 - 1L)))
                {
                    case 0:
                        num6 ^= bytes[(int)((IntPtr)(length - 1L))];
                        break;

                    case 1:
                        num6 ^= (uint)(bytes[(int)((IntPtr)(length - 1L))] << 8);
                        num6 ^= bytes[(int)((IntPtr)(length - 2L))];
                        break;

                    case 2:
                        num6 ^= (uint)(bytes[(int)((IntPtr)(length - 1L))] << 0x10);
                        num6 ^= (uint)(bytes[(int)((IntPtr)(length - 2L))] << 8);
                        num6 ^= bytes[(int)((IntPtr)(length - 3L))];
                        break;
                }
            }
            num6 *= num;
            num6 = RotateLeft(num6, 15) * num2;
            n ^= num6;
            n ^= (uint)length;
            n ^= n >> 0x10;
            n *= 0x85ebca6b;
            n ^= n >> 13;
            n *= 0xc2b2ae35;
            return (n ^ (n >> 0x10));
        }

        static uint RotateLeft(uint n, int numBits) =>
            ((n << numBits) | (n >> (0x20 - numBits)));

        //
        // Helper for Hex Encoded String
        //

        static string ToHexEncodedBinaryString(double value)
        {
            string str;
            byte[] buffer = new byte[0x150];
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    WriteForBinaryEncoding(writer, value);
                    str = ToHex(buffer, 0, (int)stream.Position);
                }
            }
            return str;
        }

        static void WriteForBinaryEncoding(BinaryWriter binaryWriter, double value)
        {
            binaryWriter.Write((byte)5);
            ulong num = EncodeDoubleAsUInt64(value);
            binaryWriter.Write((byte)(num >> 0x38));
            num = num << 8;
            byte num2 = 0;
            bool flag = true;
            do
            {
                if (!flag)
                {
                    binaryWriter.Write(num2);
                }
                else
                {
                    flag = false;
                }
                num2 = (byte)((num >> 0x38) | ((ulong)1L));
                num = num << 7;
            }
            while (num != 0L);
            binaryWriter.Write((byte)(num2 & 0xfe));
        }

        static ulong EncodeDoubleAsUInt64(double value)
        {
            ulong num = (ulong)BitConverter.DoubleToInt64Bits(value);
            ulong num2 = 9223372036854775808L;
            if (num >= num2)
            {
                return (~num + ((ulong)1L));
            }
            return (num ^ num2);
        }

        static string ToHex(byte[] bytes, int start, int length)
        {
            // create lookup table
            ushort[] lookupArray = new ushort[0x100];
            for (int i = 0; i < 0x100; i++)
            {
                string str = i.ToString("X2", CultureInfo.InvariantCulture);
                lookupArray[i] = (ushort)(str[0] + (str[1] << 8));
            }

            // ToHex
            char[] chArray = new char[length * 2];
            for (int i = 0; i < length; i++)
            {
                ushort num2 = lookupArray[bytes[i + start]];
                chArray[2 * i] = (char)(num2 & 0xff);
                chArray[(2 * i) + 1] = (char)(num2 >> 8);
            }

            return new string(chArray);
        }
    }
}
