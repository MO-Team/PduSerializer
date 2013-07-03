namespace PduSerializer.TestData
{
    [PduMessage]
    public class NoFields
    {
    }

    [PduMessage]
    public class NoFieldsWithNoFields
    {
        [Field(Position = 1)]
        public NoFields NoFields { get; set; }
    }
}