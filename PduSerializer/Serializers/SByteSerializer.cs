using System;
using System.IO;

namespace PduSerializer.Serializers
{
    internal class SByteSerializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            stream.Write((SByte) obj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return stream.ReadSByte();
        }

        #endregion
    }
}