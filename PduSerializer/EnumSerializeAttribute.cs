using System;
using  PduSerializer.Properties;
using  PduSerializer.Serializers;

namespace PduSerializer
{
    /// <summary>
    /// Supported enum value types
    /// </summary>
    public enum EnumValueType
    {
        Short,
        Int,
        Long,
        Byte
    }

    /// <summary>
    /// Defines an enum or an enum field to use the enum serializer.
    /// Default enum value type is int.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EnumSerializeAttribute : CustomSerializerAttribute
    {
        public EnumSerializeAttribute(EnumValueType enumValueType = EnumValueType.Int)
        {
            CustomSerializer = CreateSerializer(enumValueType);
        }

        private static ICustomSerializer CreateSerializer(EnumValueType enumValueType)
        {
            switch (enumValueType)
            {
                case EnumValueType.Short:
                    return new Int16Serializer();
                case EnumValueType.Int:
                    return new Int32Serializer();
                case EnumValueType.Long:
                    return new Int64Serializer();
                case EnumValueType.Byte:
                    return new ByteSerializer();
                default:
                    throw new NotSupportedException(string.Format(Messages.UnsupportedEnumValueTypeErrorMessage,
                                                                  enumValueType));
            }
        }
    }
}