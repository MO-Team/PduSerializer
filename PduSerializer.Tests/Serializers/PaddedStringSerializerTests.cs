using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;
using PduSerializer.Serializers;

namespace PduSerializer.Tests.Serializers
{
    [TestFixture]
    public class PaddedStringSerializerTests
    {
        [Test]
        public void Ctor_NegativeSize_Exception()
        {
            // Act 
            Assert.Throws<ArgumentOutOfRangeException>(() => new PaddedStringSerializer(-4, Encoding.Default));
        }

        [Test]
        public void Ctor_NullEncoding_Exception()
        {
            // Act 
            Assert.Throws<ArgumentNullException>(() => new PaddedStringSerializer(4, null));
        }

        [Test]
        public void Ctor_SizeIsZero_Exception()
        {
            // Act 
            Assert.Throws<ArgumentOutOfRangeException>(() => new PaddedStringSerializer(0, Encoding.Default));
        }

        [Test]
        public void Deserialize_OriginalStringLength_NoTrim()
        {
            //Arrange
            const int size = 3;
            var input = new Byte[] {98, 108, 97};
            const string expected = "bla";


            Encoding encoding = Encoding.Default;
            var serializer = new PaddedStringSerializer(size, encoding);

            //Act
            using (var stream = new MemoryStream(input))
            using (var reader = new BinaryReader(stream))
            {
                object actual = serializer.Deserialize(reader, null);

                //Assert
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Deserialize_SizeBiggerThanOriginalStringAlignLeft_TrimStart()
        {
            //Arrange
            const int size = 7;
            var input = new Byte[] {32, 32, 98, 108, 97, 32, 32};
            const string expected = "  bla";


            Encoding encoding = Encoding.Default;
            var serializer = new PaddedStringSerializer(size, encoding, Alignment.Left);

            //Act
            using (var stream = new MemoryStream(input))
            using (var reader = new BinaryReader(stream))
            {
                object actual = serializer.Deserialize(reader, null);

                //Assert
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Deserialize_SizeBiggerThanOriginalStringAlignRight_TrimEnd()
        {
            //Arrange
            const int size = 7;
            var input = new Byte[] {32, 32, 98, 108, 97, 32, 32};
            const string expected = "bla  ";


            Encoding encoding = Encoding.Default;
            var serializer = new PaddedStringSerializer(size, encoding);

            //Act
            using (var stream = new MemoryStream(input))
            using (var reader = new BinaryReader(stream))
            {
                object actual = serializer.Deserialize(reader, null);

                //Assert
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Serialize_SizeBiggerThanStringAlignLeft_PadGivenCharToEndOfString()
        {
            //Arrange
            const string input = "bla"; // = new Byte[]{98,108,97}
            const int size = 5;
            var expected = new Byte[] {98, 108, 97, 32, 32};


            Encoding encoding = Encoding.Default;
            var serializer = new PaddedStringSerializer(size, encoding, Alignment.Left);

            //Act
            using (var stream = new MemoryStream())
            using (var write = new BinaryWriter(stream))
            {
                serializer.Serialize(input, write, null);


                //Assert
                IEnumerable<byte> actual = stream.GetBuffer().Take(size);
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Serialize_SizeBiggerThanStringAlignRight_PadGivenCharToStartOfString()
        {
            //Arrange
            const string input = "bla"; // = new Byte[]{98,108,97}
            const int size = 5;
            var expected = new Byte[] {32, 32, 98, 108, 97};


            Encoding encoding = Encoding.Default;
            var serializer = new PaddedStringSerializer(size, encoding);

            //Act
            using (var stream = new MemoryStream())
            using (var write = new BinaryWriter(stream))
            {
                serializer.Serialize(input, write, null);


                //Assert
                IEnumerable<byte> actual = stream.GetBuffer().Take(size);
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Serialize_StringLength_WriteString()
        {
            //Arrange
            const string input = "bla"; // = new Byte[]{98,108,97}
            const int size = 3;
            var expected = new Byte[] {98, 108, 97};


            Encoding encoding = Encoding.Default;
            var serializer = new PaddedStringSerializer(size, encoding);

            //Act
            using (var stream = new MemoryStream())
            using (var write = new BinaryWriter(stream))
            {
                serializer.Serialize(input, write, null);


                //Assert
                IEnumerable<byte> actual = stream.GetBuffer().Take(size);
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Serialize_TooSmallSize_Exception()
        {
            //Arrange
            const string input = "bla";
            const int size = 2;

            Encoding encoding = Encoding.Default;
            var serializer = new PaddedStringSerializer(size, encoding);

            //Act
            using (var stream = new MemoryStream())
            using (var write = new BinaryWriter(stream))
            {
                Assert.Throws<SerializationException>(() => serializer.Serialize(input, write, null));
            }
        }
    }
}