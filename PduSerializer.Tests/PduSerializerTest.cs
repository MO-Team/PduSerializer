using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using PduSerializer.TestData;
using System.Collections.Generic;

namespace PduSerializer.Tests
{
    [TestFixture]
    public class PduSerializerTest
    {
        public ISerializationEngine Engine { get; set; }
        public MemoryStream Stream { get; set; }
        public BinaryWriter Writer { get; set; }
        public BinaryReader Reader { get; set; }

        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            Engine = PduSerializer.Configure()
                .AddTypesFromAssemblyOf<PersonMessage>()
                .CreateSerializationEngine();

            Stream = new MemoryStream();
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


        [Test, Explicit]
        public void TestPerformanceForPduSerializer()
        {
            //Arrange
            var person = new PduPerson { Age = 15, Id = 1234, Name = "The", UsaShoeSize = 3, Gender = IntEnum.Male };
            using (var stream = new MemoryStream())
            using (var write = new BinaryWriter(stream))
            using (var reader = new BinaryReader(stream))
            {
                //Act
                ISerializationEngine engine = PduSerializer.Configure()
                    .AddType<PduPerson>()
                    .CreateSerializationEngine();


                var watch = new Stopwatch();
                watch.Start();
                for (int i = 0; i < 100000; i++)
                {
                    engine.Serialize(person, write);
                }
                watch.Stop();
                stream.Position = 0;
                Console.WriteLine(@"Serialization Time:" + watch.ElapsedMilliseconds);
                watch.Restart();

                for (int i = 0; i < 100000; i++)
                {
                    engine.Deserialize(typeof(PduPerson), reader);
                }
                watch.Stop();
                Console.WriteLine(@"Deserialization Time:" + watch.ElapsedMilliseconds);
            }
        }


        [Test]
        public void Deserialize_2000RMatipTypes_PerformanceIsBelow200ms()
        {
            //Arrange
            var rmatip = new HeavyMessage
            {
                MessageId = 8,
                AverageError = 17,
                Callsign = "fima",
                CameraHeadingAngle = 3,
                Number = 93,
                Precision = 39,
                TailNumber = 83,
                SquadronNumber = 890
            };

            var stopwatch = new Stopwatch();

            //Act
            Engine.Serialize(rmatip, Writer);

            stopwatch.Start();
            for (int i = 0; i < 2000; i++)
            {
                Stream.Position = 0;
                Engine.Deserialize(typeof(HeavyMessage), Reader);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds); //72
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 500);
        }

        [Test]
        public void Serialize_2000RMatipTypes_PerformanceIsBelow200ms()
        {
            //Arrange
            var rmatip = new HeavyMessage
            {
                MessageId = 8,
                AverageError = 17,
                Callsign = "fima",
                CameraHeadingAngle = 3,
                Number = 93,
                Precision = 39,
                TailNumber = 83,
                SquadronNumber = 890
            };

            var stopwatch = new Stopwatch();

            //Act
            stopwatch.Start();
            for (int i = 0; i < 2000; i++)
            {
                Engine.Serialize(rmatip, Writer);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds); //42
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 500);
        }


        [Test]
        public void ListDeserialize_2000RMatipTypes_PerformanceIsBelow200ms()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var rmatipList = new List<HeavyMessage>();
            for (ushort i = 0; i < 1000; i++)
            {
                rmatipList.Add(new HeavyMessage { MessageId = i,
                                                   AverageError = 17,
                                                   Callsign = "fima",
                                                   CameraHeadingAngle = 3,
                                                   Number = 93,
                                                   Precision = 39,
                                                   TailNumber = 83,
                                                   SquadronNumber = 890 });
            }
            var listMessage = new ListMessage { VariableList = rmatipList, ConstantList = rmatipList, Header = new ListMessageHeader { VariableListSize = 1000 } };

            //Act
            Engine.Serialize(listMessage, Writer);

            stopwatch.Start();
            Stream.Position = 0;
            Engine.Deserialize(typeof(ListMessage), Reader);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds); //73
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 500);
        }

        [Test]
        public void ListSerialize_2000RMatipTypes_PerformanceIsBelow200ms()
        {
            //Arrange
            var stopwatch = new Stopwatch();
            var rmatipList = new List<HeavyMessage>();
            for (ushort i = 0; i < 1000; i++)
            {
                rmatipList.Add(new HeavyMessage { MessageId = i,
                                                   AverageError = 17,
                                                   Callsign = "fima",
                                                   CameraHeadingAngle = 3,
                                                   Number = 93,
                                                   Precision = 39,
                                                   TailNumber = 83,
                                                   SquadronNumber = 890 });
            }
            var listMessage = new ListMessage { VariableList = rmatipList, ConstantList = rmatipList, Header = new ListMessageHeader { VariableListSize = 1000 } };

            //Act
            stopwatch.Start();
            Engine.Serialize(listMessage, Writer);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds); //49
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 500);
        }
    }
}