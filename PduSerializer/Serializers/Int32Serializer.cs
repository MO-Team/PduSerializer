using System;
using System.IO;
using  PduSerializer.Internal;

namespace PduSerializer.Serializers
{
    internal class Int32Serializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var intObj = (Int32) obj;
            stream.Write(context.ChangeByteOrder
                             ? intObj.ChangeByteOrder()
                             : intObj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return context.ChangeByteOrder
                ? stream.ReadInt32().ChangeByteOrder() 
                : stream.ReadInt32();
        }

        #endregion
    }
}