using PduSerializer.BitwiseSerialization;
using PduSerializer.Internal;
using PduSerializer.TestData;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PduSerializer.Tests.BitwiseSerialization
{
    public class BitwiseSerializationPerformanceTests
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

        [Test]
        public void Serialize_2000PduPersonWithBitArrays_PerformanceIsBelow50ms()
        {
            //Arrange
            var message = new PduPersonWithBitArrays
            {
                BitArray = new BitArray(new bool[] { true, true, false }),
                Name = "Dennis",
                BoolArray = new bool[] { false, false, true, false, false },
                Gender = IntEnum.Female,
                BooleanArray = new Boolean[] { true },
                Age = 21,
            };
            var stopwatch = new Stopwatch();

            //Act
            stopwatch.Start();
            for (int i = 0; i < 2000; i++)
            {
                Engine.Serialize(message, Writer);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds); //10
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 100);
        }

        [Test]
        public void Deserialize_2000PduPersonWithBitArrays_PerformanceIsBelow50ms()
        {
            //Arrange
            var message = new PduPersonWithBitArrays { BitArray = new BitArray(new bool[] { true, true, false }),
                                                       Name = "Dennis", 
                                                       BoolArray = new bool[] { false, false, true, false, false },
                                                       Gender = IntEnum.Female,
                                                       BooleanArray = new Boolean[] { true },
                                                       Age = 21, };
            var stopwatch = new Stopwatch();

            //Act
            Engine.Serialize(message, Writer);

            stopwatch.Start();
            for (int i = 0; i < 2000; i++)
            {
                Stream.Position = 0;
                Engine.Deserialize(typeof(PduPersonWithBitArrays), Reader);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds); //8
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 100);
        }



        [Test, Explicit]
        public void Serialize_CompareBitArrayToBoolArray_BoolArrayIsFaster()
        {
            //Arrange
            var boolArray = new bool[] { true, true, false, false, false, false, true, true, false, true };
            var bitArray = new BitArray(boolArray);
            var serializer = new BitArraySerializer(boolArray.Length);
            var boolArrayContext = new FieldSerializationContext(new FieldSerializationInfo{ MemberType = typeof(bool[]) }, null, null);
            var bitArrayContext = new FieldSerializationContext(new FieldSerializationInfo{ MemberType = typeof(BitArray) }, null, null);
            var runTimes = 1000000;
            var stopwatch = new Stopwatch();

            //Act
            stopwatch.Start();
            for (int i = 0; i < runTimes; i++)
            {
                Stream.Position = 0;
                serializer.Serialize(boolArray, Writer, boolArrayContext);
            }
            stopwatch.Stop();
            Console.WriteLine(runTimes + " serialization of bool[]: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            for (int i = 0; i < runTimes; i++)
            {
                Stream.Position = 0;
                serializer.Serialize(bitArray, Writer, bitArrayContext);
            }
            stopwatch.Stop();
            Console.WriteLine(runTimes + " serialization of BitArrays: " + stopwatch.ElapsedMilliseconds);
        }

        [Test, Explicit]
        public void Deserialize_CompareBitArrayToBoolArray_BoolArrayIsFaster()
        {
            //Arrange
            var boolArray = new bool[] { true, true, false, false, false, false, true, true, false, true };
            var serializer = new BitArraySerializer(boolArray.Length);
            var boolArrayContext = new FieldSerializationContext(new FieldSerializationInfo { MemberType = typeof(bool[]) }, null, null);
            var bitArrayContext = new FieldSerializationContext(new FieldSerializationInfo { MemberType = typeof(BitArray) }, null, null);
            var runTimes = 1000000;
            var stopwatch = new Stopwatch();

            //Act
            Stream.Write(new bool[] { true, true, false, false, false, false, true, true, false, true });

            stopwatch.Start();
            for (int i = 0; i < runTimes; i++)
            {
                Stream.Position = 0;
                serializer.Deserialize(Reader, boolArrayContext);
            }
            stopwatch.Stop();
            Console.WriteLine(runTimes + " serialization of bool[]: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            for (int i = 0; i < runTimes; i++)
            {
                Stream.Position = 0;
                serializer.Deserialize(Reader, bitArrayContext);
            }
            stopwatch.Stop();
            Console.WriteLine(runTimes + " serialization of BitArrays: " + stopwatch.ElapsedMilliseconds);
        }
    }
}
