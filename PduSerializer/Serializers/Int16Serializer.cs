using System;
using System.IO;
using  PduSerializer.Internal;

namespace PduSerializer.Serializers
{
    internal class Int16Serializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var intObj = (Int16) obj;
            stream.Write(context.ChangeByteOrder
                             ? intObj.ChangeByteOrder()
                             : intObj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return context.ChangeByteOrder 
                ? stream.ReadInt16().ChangeByteOrder() 
                : stream.ReadInt16();
        }

        #endregion
    }
}