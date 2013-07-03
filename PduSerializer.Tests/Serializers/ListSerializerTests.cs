using NUnit.Framework;
using PduSerializer.TestData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PduSerializer.Tests.Serializers
{
    public class ListSerializerTests : BaseSerializationEngineTests
    {
        #region Constant List Tests

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Serialize_UnsupportedListType_Exception()
        {
            // Arrange
            var arrayClass = new BitArrayClass
            {
                FiveBitArray = BuildBitArray(5),
                OneBitArray = BuildBitArray(1),
                ThirtyTwoBitArray = BuildBitArray(12)
            };

            // Act
            Engine.Serialize(arrayClass, Writer);
            Stream.Position = 0;
            var desirializedArray = (BitArrayClass)Engine.Deserialize(typeof(BitArrayClass), Reader);
        }

        [Test]
        public void Serialize_FullConstArrayClass_DeserialzesTheClass()
        {
            // Arrange
            var listClass = new ConstArrayClass
            {
                IntArray = new int[] { 1, 2, 3 },
                DoubleArray = new[] { 1.1, 2.2, 3.3 }
            };

            // Act
            Engine.Serialize(listClass, Writer);
            Stream.Position = 0;
            var desirializedArray = (ConstArrayClass)Engine.Deserialize(typeof(ConstArrayClass), Reader);

            // Assert
            CollectionAssert.AreEqual(listClass.IntArray, desirializedArray.IntArray);
            CollectionAssert.AreEqual(listClass.DoubleArray, desirializedArray.DoubleArray);
        }

        private BitArray BuildBitArray(int size)
        {
            var bitArray = new BitArray(size);

            var randomBitValue = true;

            for (int i = 0; i < size; i++)
            {
                bitArray.Set(i, randomBitValue);

                randomBitValue = !randomBitValue;
            }

            return bitArray;
        }

        #endregion

        #region Variable List Tests

        [Test]
        public void Serialize_ListOfPersonMessage_SerializedToByteStream()
        {
            //Arrange
            var variableListClass = new VariableListClass
            {
                Header = new MessageHeader { PeopleWithFriendsNum = 2 },
                PeopleNum = 3,
                People = new PersonMessage[] { new PersonMessage { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true },
                                               new PersonMessage { Id = 7845, Name = "Luxi", Age = 23, WillWriteCode = true },
                                               new PersonMessage { Id = 12345, Name = "Dennis", Age = 13, WillWriteCode = false } },
                PeopleWithFriends = new List<PersonWithVariableList> { new PersonWithVariableList { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true, 
                                                                                                    FriendsNum = 1, Friends = new List<PersonMessage> { new PersonMessage { Id = 7845, Name = "Luxi", Age = 23, WillWriteCode = true } } },
                                                                       new PersonWithVariableList { Id = 12345, Name = "Dennis", Age = 13, WillWriteCode = false, 
                                                                                                    FriendsNum = 0, Friends = new List<PersonMessage>() } },
            };

            //Act
            Engine.Serialize(variableListClass, Writer);

            //Assert
            var expected = new byte[] { 2, 0, 0, 0,  // Header
                                        3, 0,  // PeopleNum
                                        5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110,  // People[0]
                                        23, 0, 0, 0, 165, 30, 0, 0, 0, 0, 0, 0, 1, 4, 76, 117, 120, 105,  // People[1]
                                        13, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 0, 6, 68, 101, 110, 110, 105, 115,  // People[2]
                                        5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110, 1, 0, 0, 0,  // PeopleWithFriends[0]
                                                23, 0, 0, 0, 165, 30, 0, 0, 0, 0, 0, 0, 1, 4, 76, 117, 120, 105,  // PeopleWithFriends[0].Friends[0]
                                        13, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 0, 6, 68, 101, 110, 110, 105, 115, 0, 0, 0, 0,  // PeopleWithFriends[1]
                                      };
            Assert.That(Stream.Length, Is.EqualTo(expected.Length));
            byte[] actual = Stream.GetBuffer().Take((int)Stream.Length).ToArray();
            Assert.That(actual, Is.EquivalentTo(expected));
        }
        

        [Test]
        public void Deserialize_ByteStreamOfVariableListClass_DeserializedToList()
        {
            //Arrange
            var buffer = new byte[] { 2, 0, 0, 0,  // Header
                                      2, 0,  // PeopleNum
                                      13, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 0, 6, 68, 101, 110, 110, 105, 115,  // People[0]
                                      5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110,  // People[1]
                                      5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110, 1, 0, 0, 0,  // PeopleWithFriends[0]
                                                23, 0, 0, 0, 165, 30, 0, 0, 0, 0, 0, 0, 1, 4, 76, 117, 120, 105,  // PeopleWithFriends[0].Friends[0]
                                      13, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 0, 6, 68, 101, 110, 110, 105, 115, 2, 0, 0, 0,  // PeopleWithFriends[1]
                                                23, 0, 0, 0, 165, 30, 0, 0, 0, 0, 0, 0, 1, 4, 76, 117, 120, 105,
                                                5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110,
                                    };

            //Act
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
            var actual = (VariableListClass)Engine.Deserialize(typeof(VariableListClass), Reader);

            //Assert
            var expected = new VariableListClass
            {
                Header = new MessageHeader { PeopleWithFriendsNum = 2 },
                PeopleNum = 2,
                People = new PersonMessage[] { new PersonMessage { Id = 12345, Name = "Dennis", Age = 13, WillWriteCode = false },
                                               new PersonMessage { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true } },
                PeopleWithFriends = new List<PersonWithVariableList> { new PersonWithVariableList { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true, 
                                                                                                    FriendsNum = 1, Friends = new List<PersonMessage> { new PersonMessage { Id = 7845, Name = "Luxi", Age = 23, WillWriteCode = true } } },
                                                                       new PersonWithVariableList { Id = 12345, Name = "Dennis", Age = 13, WillWriteCode = false, 
                                                                                                    FriendsNum = 2, Friends = new List<PersonMessage> { new PersonMessage { Id = 7845, Name = "Luxi", Age = 23, WillWriteCode = true },
                                                                                                                                                        new PersonMessage { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true } } } },
            };
            Assert.That(actual.Header, Is.EqualTo(expected.Header));
            Assert.That(actual.PeopleNum, Is.EqualTo(expected.PeopleNum));
            Assert.That(actual.People, Is.EquivalentTo(expected.People));
            Assert.That(actual.PeopleWithFriends.Count, Is.EqualTo(expected.PeopleWithFriends.Count));
            for (var i = 0; i < expected.PeopleWithFriends.Count; i++)
            {
                Assert.That(actual.PeopleWithFriends[i], Is.EqualTo(expected.PeopleWithFriends[i]).Using(new PersonWithVariableListComparer()));
            }
        }

        #endregion


        class PersonWithVariableListComparer : IComparer<PersonWithVariableList>
        {
            public int Compare(PersonWithVariableList x, PersonWithVariableList y)
            {
                Assert.That(x.Id, Is.EqualTo(y.Id));
                Assert.That(x.Name, Is.EqualTo(y.Name));
                Assert.That(x.Age, Is.EqualTo(y.Age));
                Assert.That(x.WillWriteCode, Is.EqualTo(y.WillWriteCode));
                Assert.That(x.FriendsNum, Is.EqualTo(y.FriendsNum));
                Assert.That(x.Friends, Is.EquivalentTo(y.Friends));

                return 0;
            }
        }
    }
}
