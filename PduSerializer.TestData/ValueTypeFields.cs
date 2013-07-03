namespace PduSerializer.TestData
{
    [PduMessage]
    public class ValueTypeFieldMessage
    {
        [Field] public PersonMessage ValueType;
    }

    [PduMessage]
    public class ValueTypePropertyMessage
    {
        [Field]
        public PersonMessage ValueType { get; set; }
    }
}