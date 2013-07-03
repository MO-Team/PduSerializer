using System;
using NUnit.Framework;
using PduSerializer.Internal;

namespace PduSerializer.Tests.Internals
{
    [TestFixture]
    public class ByteOrderExtentionsTests
    {
        [Test]
        public void ChangeByteOrder_Double_ReverseByteOrder()
        {
            // Arrange 
            const double input = 121212.1212; //ByteOrder is { 68,105,111,240,193,151,253,64} 
            var expectedByteOrder = new byte[] {64, 253, 151, 193, 240, 111, 105, 68};


            //Act
            double output = input.ChangeByteOrder();
            byte[] actualByteOrder = BitConverter.GetBytes(output);

            //Assert
            Assert.That(actualByteOrder, Is.EqualTo(expectedByteOrder));
        }

        [Test]
        public void ChangeByteOrder_Float_ReverseByteOrder()
        {
            // Arrange 
            const float input = (float) 121212.1212; //ByteOrder is { 16,290,236,71}
            var expectedByteOrder = new byte[] {71, 236, 190, 16};


            //Act
            float output = input.ChangeByteOrder();
            byte[] actualByteOrder = BitConverter.GetBytes(output);

            //Assert
            Assert.That(actualByteOrder, Is.EqualTo(expectedByteOrder));
        }

        [Test]
        public void ChangeByteOrder_UInt16_ReverseByteOrder()
        {
            // Arrange 
            const ushort input = 1212; //ByteOrder is {188,4}
            var expectedByteOrder = new byte[] {4, 188};


            //Act
            ushort output = input.ChangeByteOrder();
            byte[] actualByteOrder = BitConverter.GetBytes(output);

            //Assert
            Assert.That(actualByteOrder, Is.EqualTo(expectedByteOrder));
        }

        [Test]
        public void ChangeByteOrder_UInt32_ReverseByteOrder()
        {
            // Arrange 
            const uint input = 121212; //ByteOrder is {124,217,1,0}
            var expectedByteOrder = new byte[] {0, 1, 217, 124};


            //Act
            uint output = input.ChangeByteOrder();
            byte[] actualByteOrder = BitConverter.GetBytes(output);

            //Assert
            Assert.That(actualByteOrder, Is.EqualTo(expectedByteOrder));
        }

        [Test]
        public void ChangeByteOrder_UInt64_ReverseByteOrder()
        {
            // Arrange 
            const ulong input = 1212121212; //ByteOrder is { 124, 128, 63, 72,0,0,0,0 }
            var expectedByteOrder = new byte[] {0, 0, 0, 0, 72, 63, 128, 124};


            //Act
            ulong output = input.ChangeByteOrder();
            byte[] actualByteOrder = BitConverter.GetBytes(output);

            //Assert
            Assert.That(actualByteOrder, Is.EqualTo(expectedByteOrder));
        }
    }
}