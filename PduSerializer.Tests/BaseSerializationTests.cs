using NUnit.Framework;
using PduSerializer.TestData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PduSerializer.Tests
{
    public abstract class BaseSerializationEngineTests
    {
        public ISerializationEngine Engine { get; set; }
        public MemoryStream Stream { get; set; }
        public BinaryWriter Writer { get; set; }
        public BinaryReader Reader { get; set; }


        [SetUp]
        public virtual void Setup()
        {
            Engine = PduSerializer.Configure()
                                  .AddTypesFromAssemblyOf<PersonMessage>()
                                  .CreateSerializationEngine();

            Stream = new MemoryStream();
            Writer = new BinaryWriter(Stream);
            Reader = new BinaryReader(Stream);
        }

        [TearDown]
        public virtual void TearDown()
        {
            Reader.Dispose();
            Writer.Dispose();
            Stream.Dispose();
        }
    }
}
