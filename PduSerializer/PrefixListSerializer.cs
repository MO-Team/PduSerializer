using System;
using System.Collections;
using System.IO;

namespace PduSerializer
{
    public class PrefixListSerializer : ICustomSerializer
    {
  

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            stream.Write((byte)((IList)obj).Count);
            for (var i = 0; i < ((IList)obj).Count; i++)
            {
                var item = ((IList)obj)[i];
                context.SerializationEngine.Serialize(item, stream);
            }
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            var arrayElementType = context.MemberType.GetElementType();
            var size = stream.ReadByte();
            var list = Array.CreateInstance(arrayElementType, size);

            for (var i = 0; i < size; i++)
            {
                list.SetValue(context.SerializationEngine.Deserialize(arrayElementType, stream), i);
            }
            return list;
        }
    }
}