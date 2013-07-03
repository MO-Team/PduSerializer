using System;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;
using PduSerializer.Serializers;
using PduSerializer.TestData;

namespace PduSerializer.Tests
{
    [TestFixture]
    public class ConfigurationStoreTests
    {
        [Test]
        public void AddSerializer_ChangedStringSerializerToAsciiConstantString_CustomSerializerAdded()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();

            //Act
            configurationStore.AddSerializer(typeof (string), new PaddedStringSerializer(3, Encoding.ASCII));
            configurationStore.AddType(typeof (Location));

            //Assert
            Assert.IsTrue(configurationStore.TypeDefaultSerializers[typeof (string)] is PaddedStringSerializer);
        }

        [Test]
        public void AddTypeFromAssemblyOf_AssemblyWithSeveralMessageTypes_AllTypeSerializersAdded()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();

            //Act
            configurationStore.AddTypesFromAssemblyOf<HeavyMessage>();

            //Assert
            Assert.AreEqual(25, configurationStore.TypeSerializationInfo.Count);
        }

        [Test]
        public void AddTypeFromAssembly_AssemblyWithNoMessageTypes_ZeroTypeSerializersAdded()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();

            configurationStore.AddTypesFromAssemblyOf<SerializationEngine>();

            //Assert
            Assert.AreEqual(0, configurationStore.TypeSerializationInfo.Count);
        }

        [Test]
        public void AddTypeFromAssembly_AssemblyWithSeveralMessageTypes_AllTypeSerializersAdded()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();

            //Act
            configurationStore.AddTypesFromAssembly(typeof (HeavyMessage).Assembly);

            //Assert
            Assert.AreEqual(25, configurationStore.TypeSerializationInfo.Count);
        }

        [Test]
        [ExpectedException(typeof (SerializationException))]
        public void AddType_AddDateTime_ExceptionThrown()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();

            //Act
            configurationStore.AddType(typeof (DateTime));
        }

        [Test]
        public void AddType_AddMatipDateTypeAndLocation_TwoTypeSerializerAdded()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();

            //Act
            configurationStore.AddType(typeof (MatipDate));
            configurationStore.AddType(typeof (Location));

            //Assert
            Assert.AreEqual(2, configurationStore.TypeSerializationInfo.Count);
        }

        [Test]
        public void AddType_AddMatipDateTypeTwice_OneTypeSerializerAdded()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();

            //Act
            configurationStore.AddType(typeof (MatipDate));
            configurationStore.AddType(typeof (MatipDate));

            //Assert
            Assert.AreEqual(1, configurationStore.TypeSerializationInfo.Count);
        }

        [Test]
        public void AddType_AddMatipDateType_OneTypeSerializerAdded()
        {
            //Act
            var configurationStore = new ConfigurationStore();
            configurationStore.AddType(typeof (MatipDate));

            //Assert
            Assert.AreEqual(1, configurationStore.TypeSerializationInfo.Count);
        }

        [Test]
        public void AddType_CanMakeConsequentFleuntCalls_hreeTypeSerializersAdded()
        {
            //Act
            ISerializationEngine engine = PduSerializer.Configure()
                .AddType<Location>()
                .AddType<MatipDate>()
                .AddType<HeavyMessage>()
                .CreateSerializationEngine();

            //Assert
            Assert.AreEqual(3, ((SerializationEngine) engine).ConfigurationStore.TypeSerializationInfo.Count);
        }

        [Test]
        public void CreateEngine_EmptyConfiguration_SealsConfiguration()
        {
            //Arrange
            var configurationStore = new ConfigurationStore();
            configurationStore.CreateSerializationEngine();

            //Act Assert
            Assert.Throws<InvalidOperationException>(() => configurationStore.AddType(typeof (MatipDate)));
        }

        [Test]
        public void Ctor_CreatesEmpty_Serializer()
        {
            //Act
            var configurationStore = new ConfigurationStore();

            //Assert
            CollectionAssert.IsEmpty(configurationStore.TypeSerializationInfo);
        }
    }
}