using System;
using System.IO;

namespace PduSerializer
{
    /// <summary>
    /// The serialization engine that do the actual serialize and deserialize.
    /// </summary>
    /// <remarks>
    /// In order to create a new instance of <see cref="ISerializationEngine"/>, you need to create a configuration store and than create new 
    /// <see cref="ISerializationEngine"/> from it. 
    /// Use <see cref="PduSerializer.Configure()"/> to create a new configuration store and than create a <see cref="ISerializationEngine"/> with 
    /// <see cref="ISerializationConfiguration.CreateSerializationEngine()"/>.
    /// Usage example can be found in <see cref="PduSerialize"/>.
    /// </remarks>
    /// <see cref="PduSerializer"/>
    public interface ISerializationEngine
    {
        void Serialize(object obj, Stream stream);

        void Serialize(object obj, BinaryWriter writer);
        
        T Deserialize<T>(Stream stream);

        T Deserialize<T>(BinaryReader reader);

        object Deserialize(Type objectType, Stream stream);

        object Deserialize(Type objectType, BinaryReader reader);
    }
}