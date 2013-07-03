using System.IO;
using System.Runtime.Serialization;

namespace PduSerializer
{
    /// <summary>
    /// Defines a custom serializer inorder to get involve in the serialization process.
    /// </summary>
    /// <exception cref="SerializationException"/>
    public interface ICustomSerializer
    {
        void Serialize(object obj, BinaryWriter stream, ISerializationContext context);

        object Deserialize(BinaryReader stream, ISerializationContext context);
    }
}