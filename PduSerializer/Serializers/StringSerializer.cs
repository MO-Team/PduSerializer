using System.IO;

namespace PduSerializer.Serializers
{
    internal class StringSerializer : ICustomSerializer
    {
        #region ICustomSerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var str = obj as string ?? string.Empty;
            stream.Write(str);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            return stream.ReadString();
        }

        #endregion
    }
}