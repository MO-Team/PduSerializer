using  PduSerializer.Internal.Reflection.MemberAccessor;
using System;

namespace PduSerializer
{
    /// <summary>
    /// Context information for serialization operations
    /// </summary>
    public interface ISerializationContext
    {
        /// <summary>
        /// Whether to reverse the byre order.
        /// </summary>
        bool ChangeByteOrder { get; }

        /// <summary>
        /// The type of the member that is currently serialized or deserialized.
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        /// The ISerializationEngine.
        /// </summary>
        ISerializationEngine SerializationEngine { get; }

        /// <summary>
        /// Get the value of a member in the current serialized object.
        /// </summary>
        /// <param name="memberPath">The name of the member, or a path to it.</param>
        /// <returns>The member value.</returns>
        /// <example>
        /// [PduMessage]
        /// public struct VariableListClass
        /// {
        ///     [Field(Position = 1)]
        ///     public MyHeader Header { get; set; }
        /// }
        ///
        /// var value = GetContainerMember("Header.MyMember");
        /// </example>
        object GetObjectMemberValue(string memberPath);
    }
}