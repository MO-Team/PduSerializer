using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PduSerializer.BitwiseSerialization;
using System.Collections;

namespace PduSerializer.TestData
{
    [PduMessage]
    public struct PduPersonWithBitArrays
    {
        [Field(Position = 1), BitArray(3)]
        public BitArray BitArray;
        [Field(Position = 2), PaddedString(10, Alignment.Left, (char)0)]
        public string Name;
        [Field(Position = 3), BitArray(5)]
        public bool[] BoolArray;
        [Field(Position = 4), EnumSerialize]
        public IntEnum Gender;
        [Field(Position = 5), BitArray(1)]
        public Boolean[] BooleanArray;
        [Field(Position = 6)]
        public int Age;
    }
}
