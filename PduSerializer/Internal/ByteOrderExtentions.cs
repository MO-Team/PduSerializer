using System;
using System.Collections;
using System.Collections.Generic;

namespace PduSerializer.Internal
{
    /// <summary>
    /// Change byte order extention methods for basic types
    /// </summary>
    public static class ByteOrderExtentions
    {
        public static ushort ChangeByteOrder(this ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToUInt16(reversedByes, 0);
        }

        public static uint ChangeByteOrder(this uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToUInt32(reversedByes, 0);
        }

        public static ulong ChangeByteOrder(this ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToUInt64(reversedByes, 0);
        }


        public static short ChangeByteOrder(this short value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToInt16(reversedByes, 0);
        }

        public static IList ChangeByteOrder(this IList value)
        {
            var list = new ArrayList(value.Count);

            for (var currentBit = 0; currentBit < value.Count; currentBit++)
            {
                var currentValue = value[GetReversedtIndex(value, currentBit)];

                list.Add(currentValue);
            }

            return list;
        }


        public static int ChangeByteOrder(this int value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToInt32(reversedByes, 0);
        }

        public static BitArray ChangeByteOrder(this BitArray value)
        {
            var reversedBitArray = new BitArray(value.Length);

            for (var currentBit = 0; currentBit < value.Length; currentBit++)
            {
                var currentValue = value.Get(GetReversedtIndex(value, currentBit));

                reversedBitArray.Set(currentBit, currentValue);
            }

            return reversedBitArray;
        }

        public static long ChangeByteOrder(this long value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToInt64(reversedByes, 0);
        }

        public static double ChangeByteOrder(this double value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToDouble(reversedByes, 0);
        }

        private static Byte[] ReverseByteArray(Byte[] bytes)
        {
            var revesedBytes = new Byte[bytes.Length];
            for (var i = 0; i < bytes.Length; i++)
            {
                revesedBytes[i] = bytes[bytes.Length - i - 1];
            }

            return revesedBytes;
        }

        private static int GetReversedtIndex(BitArray value, int bitIndex)
        {
            return value.Length - 1 - bitIndex;
        }

        private static int GetReversedtIndex(IList value, int bitIndex)
        {
            return value.Count - 1 - bitIndex;
        }

        public static float ChangeByteOrder(this float value)
        {
            var bytes = BitConverter.GetBytes(value);
            var reversedByes = ReverseByteArray(bytes);
            return BitConverter.ToSingle(reversedByes, 0);
        }
    }
}