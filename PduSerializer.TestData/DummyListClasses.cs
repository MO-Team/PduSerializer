using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PduSerializer.TestData
{
    [PduMessage]
    public struct ConstArrayClass
    {
        [Field(Position = 1)]
        [ConstantList(3)]
        public int[] IntArray;

        [Field(Position = 2)]
        [ConstantList(3)]
        public double[] DoubleArray;
    }

    [PduMessage]
    public struct BitArrayClass
    {
        [Field(Position = 1)]
        [ConstantList(5)]
        public BitArray FiveBitArray;

        [Field(Position = 2)]
        [ConstantList(1)]
        public BitArray OneBitArray;

        [Field(Position = 3)]
        [ConstantList(12)]
        public BitArray ThirtyTwoBitArray;
    }

    [PduMessage]
    public struct VariableListClass
    {
        [Field(Position = 1)]
        public MessageHeader Header { get; set; }

        [Field(Position = 2)]
        public ushort PeopleNum;

        [Field(Position = 3)]
        [VariableList("PeopleNum")]
        public PersonMessage[] People;

        [Field(Position = 4)]
        [VariableList("Header.PeopleWithFriendsNum")]
        public List<PersonWithVariableList> PeopleWithFriends;
    }

    [PduMessage]
    public struct MessageHeader
    {
        [Field(Position = 1)]
        public int PeopleWithFriendsNum;
    }


    [PduMessage]
    public struct ListMessageHeader
    {
        [Field(Position = 1)]
        public int VariableListSize { get; set; }
    }

    [PduMessage]
    public struct ListMessage
    {
        [Field(Position = 1)]
        public ListMessageHeader Header { get; set; }

        [Field(Position = 2)]
        [VariableList("Header.VariableListSize")]
        public List<HeavyMessage> VariableList;

        [Field(Position = 3)]
        [ConstantList(1000)]
        public List<HeavyMessage> ConstantList;
    }
}
