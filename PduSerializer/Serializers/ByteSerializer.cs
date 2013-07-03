using System;
using System.IO;

namespace PduSerializer.Serializers
{
    internal class ByteSerializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            stream.Write((Byte) obj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return stream.ReadByte();
        }

        #endregion
    }
}