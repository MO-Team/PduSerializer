using System;

namespace PduSerializer
{
    /// <summary>
    /// Base class for an attribute that defines a spesific serializer for a field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class CustomSerializerAttribute : Attribute
    {
        public ICustomSerializer CustomSerializer { get; protected set; }
    }
}