using PduSerializer.BitwiseSerialization;
using PduSerializer.Internal;
using PduSerializer.TestData;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PduSerializer.Tests.BitwiseSerialization
{
    [TestFixture]
    public class BitArraySerializerTests
    {
        #region Properties

        public ISerializationEngine Engine { get; set; }
        public BitStream Stream { get; set; }
        public BinaryWriter Writer { get; set; }
        public BinaryReader Reader { get; set; }

        #endregion

        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            Engine = PduSerializer.Configure()
                                  .AddTypesFromAssemblyOf<PduPersonWithBitArrays>()
                                  .CreateSerializationEngine();

            Stream = new BitStream();
            Writer = new BinaryWriter(Stream);
            Reader = new BinaryReader(Stream);
        }

        [TearDown]
        public void TearDown()
        {
            Reader.Dispose();
            Writer.Dispose();
            Stream.Dispose();
        }

        #endregion

        #region Serialize Tests

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Serialize_StreamIsNotBitStream_ThrowsException()
        {
            // arrange
            var serializer = new BitArraySerializer(5);
            var data = new bool[] { true, true, false, true, false };
            var stream = new MemoryStream();
            Writer = new BinaryWriter(stream);

            // act
            serializer.Serialize(data, Writer, CreateSerializationContext(data));
        }

        [Test]
        public void Serialize_BoolArray_SerializedToStream()
        {
            // arrange
            var serializer = new BitArraySerializer(5);
            var data = new bool[5] { true, true, false, true, false };
            var actualData = new bool[5];

            // act
            serializer.Serialize(data, Writer, CreateSerializationContext(data));

            // assert
            Stream.Position = 0;
            Stream.Read(actualData);
            
            Assert.That(actualData, Is.EqualTo(data));
        }

        [Test]
        public void Serialize_BitArray_SerializedToStream()
        {
            // arrange
            var serializer = new BitArraySerializer(5);
            var data = new BitArray(new bool[5] { true, true, false, true, false });
            var actualData = new bool[5];

            // act
            serializer.Serialize(data, Writer, CreateSerializationContext(data));

            // assert
            Stream.Position = 0;
            Stream.Read(actualData);

            Assert.That(actualData, Is.EqualTo(data.OfType<bool>()));
        }

        [Test]
        public void Serialize_FullMessageWithBitArraysAndOtherTypes_SerializedToStream()
        {
            // arrange
            var message = new PduPersonWithBitArrays { BitArray = new BitArray(new bool[] { true, true, false }),
                                                       Name = "Dennis", 
                                                       BoolArray = new bool[] { false, false, true, false, false },
                                                       Gender = IntEnum.Female,
                                                       BooleanArray = new Boolean[] { true },
                                                       Age = 21, };
            var expected = new int[] { 1,1,0,
                                       0,1,0,0,0,1,0,0, 0,1,1,0,0,1,0,1, 0,1,1,0,1,1,1,0, 0,1,1,0,1,1,1,0, 0,1,1,0,1,0,0,1, 0,1,1,1,0,0,1,1, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
                                       0,0,1,0,0,
                                       0,0,0,0,0,0,0,1, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
                                       1,
                                       0,0,0,1,0,1,0,1, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0 }
                                    .Select(x => Convert.ToBoolean(x)).ToArray();
            var actual = new bool[expected.Length];

            // act
            Engine.Serialize(message, Writer);
            

            // assert
            Stream.Position = 0;
            Stream.Read(actual);

            Assert.That(Stream.Length, Is.EqualTo(expected.Length));
            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Deserialize Tests

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Deserialize_StreamIsNotBitStream_ThrowsException()
        {
            // arrange
            var serializer = new BitArraySerializer(5);
            var stream = new MemoryStream();
            Reader = new BinaryReader(stream);

            // act
            stream.Write(new byte[] { 8 }, 0, 1);
            stream.Position = 0;

            serializer.Deserialize(Reader, CreateSerializationContext<bool[]>());
        }

        [Test]
        public void Deserialize_BoolArray_ReturedDeserializedObject()
        {
            // arrange
            var serializer = new BitArraySerializer(10);
            var data = new bool[10] { false, true, false, true, true, true, false, false, false, true };
            object actualData;

            // act
            Stream.Write(data);

            // assert
            Stream.Position = 0;
            actualData = serializer.Deserialize(Reader, CreateSerializationContext<Boolean[]>());

            Assert.That(actualData, Is.EqualTo(data));
        }

        [Test]
        public void Deserialize_BitArray_ReturedDeserializedObject()
        {
            // arrange
            var serializer = new BitArraySerializer(10);
            var data = new bool[10] { false, true, true, false, true, true, false, false, false, true };
            object actualData;

            // act
            Stream.Write(data);

            // assert
            Stream.Position = 0;
            actualData = serializer.Deserialize(Reader, CreateSerializationContext<BitArray>());
            
            Assert.That(actualData, Is.EqualTo(new BitArray(data)));
        }

        [Test]
        public void Deserialize_FullMessageWithBitArraysAndOtherTypes_ReturedDeserializedObject()
        {
            // arrange
            var bitsData = new int[] { 0,0,1,
                                       0,1,0,0,1,1,0,0, 0,1,1,1,0,1,0,1, 0,1,1,1,1,0,0,0, 0,1,1,0,1,0,0,1, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
                                       0,1,0,0,1,
                                       0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,
                                       0,
                                       0,0,0,1,1,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0 }
                                    .Select(x => Convert.ToBoolean(x)).ToArray();
            var expected = new PduPersonWithBitArrays { BitArray = new BitArray(new bool[] { false, false, true }),
                                                       Name = "Luxi", 
                                                       BoolArray = new bool[] { false, true, false, false, true },
                                                       Gender = IntEnum.Male,
                                                       BooleanArray = new Boolean[] { false },
                                                       Age = 24, };

            // act
            Stream.Write(bitsData);
            Stream.Position = 0;
            var actual = Engine.Deserialize<PduPersonWithBitArrays>(Reader);

            // assert
            Assert.That(actual.BitArray, Is.EqualTo(expected.BitArray));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.BoolArray, Is.EqualTo(expected.BoolArray));
            Assert.That(actual.Gender, Is.EqualTo(expected.Gender));
            Assert.That(actual.BooleanArray, Is.EqualTo(expected.BooleanArray));
            Assert.That(actual.Age, Is.EqualTo(expected.Age));
        }

        #endregion

        #region Help Methods

        private ISerializationContext CreateSerializationContext<T>(T obj = default(T))
        {
            var fieldInfo = new FieldSerializationInfo{ MemberType = typeof(T) };
            return new FieldSerializationContext(fieldInfo, new SerializationEngine(new ConfigurationStore()), null);
        }

        #endregion
    }
}
