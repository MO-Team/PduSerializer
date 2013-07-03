using System.Collections.Generic;
namespace PduSerializer.TestData
{
    [PduMessage]
    public struct PersonMessage
    {
        [Field(Position = 1)] public int Age;
        [Field(Position = 2)] public ulong Id;
        [Field(Position = 3)] public bool WillWriteCode;
        [Field(Position = 4)] public string Name { get; set; }
    }

    [PduMessage]
    public struct PersonWithVariableList
    {
        [Field(Position = 1)] public int Age;
        [Field(Position = 2)] public ulong Id;
        [Field(Position = 3)] public bool WillWriteCode;
        [Field(Position = 4)] public string Name { get; set; }
        [Field(Position = 5)] public int FriendsNum { get; set; }
        [Field(Position = 6), VariableList("FriendsNum")] public IList<PersonMessage> Friends { get; set; }
    }
}