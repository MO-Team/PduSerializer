using System;
using System.IO;
using  PduSerializer.Internal;

namespace PduSerializer.Serializers
{
    internal class UInt32Serializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var intObj = (UInt32) obj;
            stream.Write(context.ChangeByteOrder
                             ? intObj.ChangeByteOrder()
                             : intObj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return context.ChangeByteOrder
                       ? stream.ReadUInt32().ChangeByteOrder()
                       : stream.ReadUInt32();
        }

        #endregion
    }
}