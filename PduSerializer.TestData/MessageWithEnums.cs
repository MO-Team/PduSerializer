namespace PduSerializer.TestData
{
    [PduMessage]
    public class MessageWithShortEnum
    {
        [Field(Position = 1)]
        public ShortEnum ShortEnum { get; set; }
    }

    [EnumSerialize(EnumValueType.Short)]
    public enum ShortEnum : short
    {
        First,
        Second
    }

    [PduMessage]
    public class MessageWithIntEnum
    {
        [Field(Position = 1), EnumSerialize]
        public IntEnum IntEnum { get; set; }
    }

    public enum IntEnum
    {
        Male,
        Female
    }

    [PduMessage]
    public class MessageWithLongEnum
    {
        [Field(Position = 1), EnumSerialize(EnumValueType.Long)]
        public LongEnum LongEnum { get; set; }
    }

    public enum LongEnum : long
    {
        First,
        Second
    }

    [PduMessage]
    public class MessageWithByteEnum
    {
        [Field(Position = 1), EnumSerialize(EnumValueType.Byte)]
        public ByteEnum ByteEnum { get; set; }
    }

    public enum ByteEnum : byte
    {
        First,
        Second
    }
}