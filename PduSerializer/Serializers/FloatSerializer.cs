using System.IO;
using  PduSerializer.Internal;

namespace PduSerializer.Serializers
{
    internal class FloatSerializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var floatObj = (float) obj;
            stream.Write(context.ChangeByteOrder
                             ? floatObj.ChangeByteOrder()
                             : floatObj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return context.ChangeByteOrder
                ? stream.ReadSingle().ChangeByteOrder() 
                : stream.ReadSingle();
        }

        #endregion
    }
}