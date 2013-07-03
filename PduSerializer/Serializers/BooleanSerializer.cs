using System;
using System.IO;

namespace PduSerializer.Serializers
{
    internal class BooleanSerializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            stream.Write((Boolean) obj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return stream.ReadBoolean();
        }

        #endregion
    }
}