using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PduSerializer.BitwiseSerialization
{
    /// <summary>
    /// Defines the field as a BitArray or Boolean array field with const size
    /// </summary>
    public class BitArrayAttribute : CustomSerializerAttribute
    {
        public BitArrayAttribute(int arraySize)
        {
            CustomSerializer = new BitArraySerializer(arraySize);
        }
    }
}
