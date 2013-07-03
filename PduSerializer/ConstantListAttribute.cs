using PduSerializer.Serializers;
using System;

namespace PduSerializer
{
    /// <summary>
    /// Defines the field as an array or list field with const size
    /// </summary>
    public class ConstantListAttribute : CustomSerializerAttribute
    {
        /// <summary>
        /// Defines the field as an array or list field with const size
        /// </summary>
        /// <param name="listSize">The number of elements in the array or list</param>
        public ConstantListAttribute(int listSize)
        {
            CustomSerializer = new ConstantListSerializer(listSize);
        }
    }
}