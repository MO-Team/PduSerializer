using System;
using System.IO;
using  PduSerializer.Internal;

namespace PduSerializer.Serializers
{
    internal class UInt64Serializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var intObj = (UInt64) obj;
            stream.Write(context.ChangeByteOrder
                             ? intObj.ChangeByteOrder()
                             : intObj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return context.ChangeByteOrder
                       ? stream.ReadUInt64().ChangeByteOrder()
                       : stream.ReadUInt64();
        }

        #endregion
    }
}