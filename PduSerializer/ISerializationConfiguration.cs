using System;

namespace PduSerializer
{
    public interface ISerializationConfiguration : ITypeConfiguration
    {
        /// <summary>
        /// Register a custom serializer as default serializer for a given type.
        /// </summary>
        /// <param name="type">Serialization type</param>
        /// <param name="serializer">serializer</param>
        /// <returns></returns>
        ISerializationConfiguration AddSerializer(Type type, ICustomSerializer serializer);

        /// <summary>
        /// Seals the configuration store, and creates a new serialization engine based on this store.
        /// </summary>
        /// <returns></returns>
        ISerializationEngine CreateSerializationEngine();
    }
}