using System;
using System.IO;
using  PduSerializer.Internal;

namespace PduSerializer.Serializers
{
    internal class DoubleSerializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var doubleObj = (Double) obj;

            stream.Write(context.ChangeByteOrder
                             ? doubleObj.ChangeByteOrder()
                             : doubleObj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return context.ChangeByteOrder
                ? stream.ReadDouble().ChangeByteOrder() 
                : stream.ReadDouble();
        }

        #endregion
    }
}