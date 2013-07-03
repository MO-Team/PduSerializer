using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;
using PduSerializer.Internal;
using PduSerializer.TestData;

namespace PduSerializer.Tests
{
    [TestFixture]
    public class SerializationEngineTests : BaseSerializationEngineTests
    {
        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void Deserialize_InvalidType_ExceptionThrown()
        {
            //Arrange
            var buffer = new byte[] { 5, 68, 111, 114, 111, 110, 5, 0, 0, 0, 1, 57, 48, 0, 0, 0, 0, 0, 0 };
            //Act
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
            Engine.Deserialize(typeof(InvalidMessage), Reader);
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void Deserialize_MessageWithInvalidInnerType_ExceptionThrown()
        {
            //Arrange
            var buffer = new byte[] { 5, 68, 111, 114, 111, 110, 5, 0, 0, 0, 1, 57, 48, 0, 0, 0, 0, 0, 0 };

            //Act
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
            Engine.Deserialize(typeof(MessageWithUnknownInnerType), Reader);
        }

        [Test]
        public void Deserialize_SimplePersonType_SerializedToByteStream()
        {
            //Arrange
            var buffer = new byte[] { 5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110 };
            var expectedperson = new PersonMessage
                                     {
                                         Id = 12345,
                                         Name = "Doron",
                                         Age = 5,
                                         WillWriteCode = true
                                     };


            //Act
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
            object actual = Engine.Deserialize(typeof(PersonMessage), Reader);

            Assert.That(actual, Is.EqualTo(expectedperson));
        }

        [Test]
        public void GenericDeserialize_SimplePersonType_SerializedToByteStream()
        {
            //Arrange
            var buffer = new byte[] { 5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110 };
            var expectedperson = new PersonMessage
            {
                Id = 12345,
                Name = "Doron",
                Age = 5,
                WillWriteCode = true
            };


            //Act
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
            object actual = Engine.Deserialize<PersonMessage>(Reader);

            Assert.That(actual, Is.EqualTo(expectedperson));
        }

        [Test]
        public void Deserialize_TwoMessages_SerializedToByteStream()
        {
            //Arrange
            var buffer = new byte[]
                             {
                                 5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110,
                                 22, 0, 0, 0, 165, 30, 0, 0, 0, 0, 0, 0, 1, 3, 76, 117, 120
                             };
            var expectedFirstPerson = new PersonMessage
                                          {
                                              Id = 12345,
                                              Name = "Doron",
                                              Age = 5,
                                              WillWriteCode = true
                                          };

            var expectedSecondPerson = new PersonMessage
                                           {
                                               Id = 7845,
                                               Name = "Lux",
                                               Age = 22,
                                               WillWriteCode = true
                                           };

            //Act
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
            object actualFirst = Engine.Deserialize(typeof(PersonMessage), Reader);
            object actualSecond = Engine.Deserialize(typeof(PersonMessage), Reader);

            Assert.That(actualFirst, Is.EqualTo(expectedFirstPerson));
            Assert.That(actualSecond, Is.EqualTo(expectedSecondPerson));
        }

        [Test]
        public void MessageWithByteEnum()
        {
            // Arrange
            var expected = new MessageWithByteEnum { ByteEnum = ByteEnum.Second };

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            var actual = (MessageWithByteEnum)Engine.Deserialize(typeof(MessageWithByteEnum), Reader);

            //Assert
            Assert.That(actual.ByteEnum, Is.EqualTo(expected.ByteEnum));
        }

        [Test]
        public void MessageWithFieldWithNoFields()
        {
            // Arrange
            var expected = new NoFieldsWithNoFields { NoFields = new NoFields() };

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            object actual = Engine.Deserialize(typeof(NoFieldsWithNoFields), Reader);

            //Assert
            Assert.That(actual, Is.InstanceOf<NoFieldsWithNoFields>());
        }

        [Test]
        public void MessageWithIntEnum()
        {
            // Arrange
            var expected = new MessageWithIntEnum { IntEnum = IntEnum.Male };

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            var actual = (MessageWithIntEnum)Engine.Deserialize(typeof(MessageWithIntEnum), Reader);

            //Assert
            Assert.That(actual.IntEnum, Is.EqualTo(expected.IntEnum));
        }

        [Test]
        public void MessageWithLongEnum()
        {
            // Arrange
            var expected = new MessageWithLongEnum { LongEnum = LongEnum.Second };

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            var actual = (MessageWithLongEnum)Engine.Deserialize(typeof(MessageWithLongEnum), Reader);

            //Assert
            Assert.That(actual.LongEnum, Is.EqualTo(expected.LongEnum));
        }

        [Test]
        public void MessageWithNoFields()
        {
            // Arrange
            var expected = new NoFields();

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            object actual = Engine.Deserialize(typeof(NoFields), Reader);

            //Assert
            Assert.That(actual, Is.InstanceOf<NoFields>());
        }

        [Test]
        public void MessageWithNullString_EmptyString()
        {
            // Arrange
            var expected = new MessageWithStringField();

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            var actual = (MessageWithStringField)Engine.Deserialize(typeof(MessageWithStringField), Reader);

            //Assert
            expected.StringField = String.Empty;
            Assert.That(actual.StringField, Is.EqualTo(expected.StringField));
        }

        [Test]
        public void MessageWithShortEnum()
        {
            // Arrange
            var expected = new MessageWithShortEnum { ShortEnum = ShortEnum.Second };

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            var actual = (MessageWithShortEnum)Engine.Deserialize(typeof(MessageWithShortEnum), Reader);

            //Assert
            Assert.That(actual.ShortEnum, Is.EqualTo(expected.ShortEnum));
        }

        [Test]
        public void MessageWithValueTypeField()
        {
            // Arrange
            var person = new PersonMessage { Id = 3, Age = 2, Name = "doron", WillWriteCode = true };
            var expected = new ValueTypeFieldMessage { ValueType = person };

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            var actual = (ValueTypeFieldMessage)Engine.Deserialize(typeof(ValueTypeFieldMessage), Reader);

            //Assert
            Assert.That(actual.ValueType, Is.EqualTo(expected.ValueType));
        }

        [Test]
        public void MessageWithValueTypeProperty()
        {
            // Arrange
            var person = new PersonMessage { Id = 3, Age = 2, Name = "doron", WillWriteCode = true };
            var expected = new ValueTypePropertyMessage { ValueType = person };

            //Act
            Engine.Serialize(expected, Writer);
            Stream.Position = 0;
            var actual = (ValueTypePropertyMessage)Engine.Deserialize(typeof(ValueTypePropertyMessage), Reader);

            //Assert
            Assert.That(actual.ValueType, Is.EqualTo(expected.ValueType));
        }

        [Test]
        public void Serialize_ConvetByteOrderMessage_SerializedWithConvertByteOrder()
        {
            // Arrange
            var expected = new ConvetByteOrderValueTypesMessage
                               {
                                   IntField = int.MinValue,
                                   UintField = uint.MaxValue,
                                   FloatField = float.MinValue,
                                   DoubleField = double.MinValue,
                                   UshortField = ushort.MaxValue,
                                   ShortField = short.MinValue,
                                   UlongField = ulong.MaxValue,
                                   LongField = long.MinValue,
                                   Bytes = Byte.MinValue,
                                   Sbytes = SByte.MinValue,
                                   Gender = IntEnum.Male,
                               };

            // Act
            Engine.Serialize(expected, Stream);
            var newStream = new MemoryStream(Stream.GetBuffer());
            object result = Engine.Deserialize(typeof(ConvetByteOrderValueTypesMessage), newStream);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void Serialize_DoubleOrderedMessage_ExceptionThrown()
        {
            //Arrange
            var person = new WrongOrderMessage { Id = 67, Name = "Doron", Age = 5, WillWriteCode = true };

            //Act
            Engine.Serialize(person, Writer);
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void Serialize_InvalidType_ExceptionThrown()
        {
            //Arrange
            var person = new InvalidMessage { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true };
            //Act
            Engine.Serialize(person, Writer);
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void Serialize_MessageWithInvalidInnerType_ExceptionThrown()
        {
            //Arrange
            var person = new MessageWithUnknownInnerType { DateBorn = DateTime.Now, Name = "Doron", Age = 5, WillWriteCode = true };
            //Act
            Engine.Serialize(person, Writer);
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void Serialize_NoTypeRegistered_ExceptionThrown()
        {
            //Arrange
            var person = new InvalidMessage { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true };

            //Act
            Engine.Serialize(person, Writer);
        }

        [Test]
        public void Serialize_RMatipType_SerializedToByteStream()
        {
            //Arrange
            var expected = new HeavyMessage
                               {
                                   MessageId = 8,
                                   AverageError = 17,
                                   Callsign = "fima",
                                   OperationName = "blablaa",
                                   OperCallsign = "blablaa",
                                   CameraHeadingAngle = 3,
                                   Number = 93,
                                   Precision = 39,
                                   TailNumber = 83,
                                   SquadronNumber = 890
                               };

            //Act
            Engine.Serialize(expected, Stream);
            var newStream = new MemoryStream(Stream.GetBuffer());
            object actual = Engine.Deserialize<HeavyMessage>(newStream);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Serialize_SimplePersonType_SerializedToByteStream()
        {
            //Arrange
            var person = new PersonMessage { Id = 12345, Name = "Doron", Age = 5, WillWriteCode = true };

            //Act
            Engine.Serialize(person, Writer);
            var expected = new byte[] { 5, 0, 0, 0, 57, 48, 0, 0, 0, 0, 0, 0, 1, 5, 68, 111, 114, 111, 110 };
            byte[] buffer = Stream.GetBuffer();
            Assert.AreEqual(expected.Length, Stream.Length);
            CollectionAssert.IsSubsetOf(expected, buffer);
        }
    }

    #region InvalidStubs

    public struct InvalidMessage
    {
        public int Age;
        public ulong Id;
        public bool WillWriteCode;
        public string Name { get; set; }
    }

    [PduMessage]
    public struct WrongOrderMessage
    {
        [Field(Position = 1)]
        public Int64 Age;
        [Field(Position = 4)]
        public int Id;
        [Field(Position = 3)]
        public bool WillWriteCode;

        [Field(Position = 1)]
        public string Name { get; set; }
    }

    [PduMessage]
    public struct MessageWithUnknownInnerType
    {
        [Field(Position = 2)]
        public Int64 Age;
        [Field(Position = 4)]
        public DateTime DateBorn;
        [Field(Position = 3)]
        public bool WillWriteCode;

        [Field(Position = 1)]
        public string Name { get; set; }
    }

    #endregion
}