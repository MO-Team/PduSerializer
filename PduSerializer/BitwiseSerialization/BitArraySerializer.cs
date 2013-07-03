using PduSerializer.Properties;
using PduSerializer.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PduSerializer.BitwiseSerialization
{
    internal class BitArraySerializer : ICustomSerializer
    {
        public BitArraySerializer(int arraySize)
        {
            if (arraySize <= 0)
                throw new ArgumentOutOfRangeException("arraySize", Messages.ListSizeOutOfRangeErrorMessage);

            _arraySize = arraySize;
        }

        private int _arraySize;

        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var bitStream = stream.BaseStream as BitStream;
            if (bitStream == null)
                throw new ArgumentException(Messages.BitStreamRequiredForBitwiseSerialization, "stream");

            bool[] bits;

            if (context.MemberType.Equals(typeof(bool[])))
                bits = (bool[])obj;
            else if (context.MemberType.Equals(typeof(BitArray)))
                bits = ((BitArray)obj).OfType<bool>().ToArray();
            else
                throw new InvalidOperationException(String.Format(Messages.BitwiseSerializationDoesNotSupportMemberType, context.MemberType.FullName));

            bitStream.Write(bits);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            var bitStream = stream.BaseStream as BitStream;
            if (bitStream == null)
                throw new ArgumentException(Messages.BitStreamRequiredForBitwiseSerialization, "stream");

            var bits = new bool[_arraySize];
            bitStream.Read(bits);
            
            if (context.MemberType.Equals(typeof(bool[])))
                return bits;
            if (context.MemberType.Equals(typeof(BitArray)))
                return new BitArray(bits);
            else
                throw new InvalidOperationException(String.Format(Messages.BitwiseSerializationDoesNotSupportMemberType, context.MemberType.FullName));
        }

        #endregion
    }
}
