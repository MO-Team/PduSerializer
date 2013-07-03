namespace PduSerializer.TestData
{
    [PduMessage(ChangeByteOrder = true)]
    public struct ConvetByteOrderValueTypesMessage
    {
        [Field(Position = 1)] public byte Bytes;
        [Field(Position = 2)] public double DoubleField;
        [Field(Position = 3)] public float FloatField;
        [Field(Position = 4), EnumSerialize] public IntEnum Gender;
        [Field(Position = 5)] public int IntField;
        [Field(Position = 6)] public long LongField;
        [Field(Position = 7)] public sbyte Sbytes;
        [Field(Position = 8)] public short ShortField;
        [Field(Position = 9)] public uint UintField;
        [Field(Position = 10)] public ulong UlongField;
        [Field(Position = 11)] public ushort UshortField;
    }

    [PduMessage(ChangeByteOrder = true)]
    public class DoubleConvertByteOrderMessage
    {
        public StubClass Stub { get; set; }
    }

    [PduMessage(ChangeByteOrder = true)]
    public class StubClass
    {
        [Field(Position = 1), EnumSerialize] public IntEnum Gender;

        [Field(Position = 2), EnumSerialize]
        public IntEnum Gender2 { get; set; }
    }
}