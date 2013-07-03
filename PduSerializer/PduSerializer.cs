using System;
using System.IO;

namespace PduSerializer
{
    /// <summary>
    /// The main entry point for configuration and creation of a new <see cref="ISerializationEngine"/>.
    /// </summary>
    /// <example>
    /// To serialize a class or struct, it must have a <see cref="PduMessageAttribute"/> on it. 
    /// Any field or property that should be serialized must contain <see cref="FieldAttribute"/> attribute, and declare its position.
    /// 
    /// For example:
    /// 
    ///     [PduMessage]
    ///     public class PduPerson
    ///     {
    ///         [Field(Position = 1)]
    ///         public string Name;
    ///         [Field(Position = 2)]
    ///         public string LastName;
    ///         [Field(Position = 3)]
    ///         public int Age;
    ///         [Field(Position = 4)]
    ///         public int Id;
    ///         [Field(Position = 5)]
    ///         public double UsaShoeSize;
    ///         [Field(Position = 6), EnumSerialize]
    ///         public IntEnum Gender;
    ///     }
    /// 
    /// 
    /// To serialize or deserialize an object, configure a new serialization engine, like the example bellow.
    ///     
    ///     ISerializationEngine engine = PduSerializer.Configure()
    ///                                                               .AddTypesFromAssemblyOf<PersonMessage>()
    ///                                                               .CreateSerializationEngine();
    /// 
    /// 
    /// To serialize an object to binary stream, create a new stream that will be used for serialization, like the example bellow.
    /// 
    ///     var person = new PduPerson() { Name = "Dennis", LastName = "Nerush", ... };
    ///     
    ///     using (var stream = new MemoryStream())
    ///     {
    ///         engine.Serialize(person, stream);
    ///         
    ///         // pass the binary stream to server, client etc.
    ///     }
    /// 
    /// 
    /// To de-serialize a from binary stream to object, create a new stream that will be used for serialization, like the example bellow.
    /// 
    ///     PduPerson person;
    ///     
    ///     using (var stream = new MemoryStream())
    ///     {
    ///         // write into stream from binary data from server, client etc.
    ///         
    ///         person = engine.Deserialize<PduPerson>(stream);
    ///     }
    /// </example>
    /// <remarks>
    /// The configuration and creation is expensive operation.
    /// It is recommended to configure and create <see cref="ISerializationEngine"/> only once and use the same instance for all serializations.
    /// </remarks>
    public static class PduSerializer
    {
        /// <summary>
        /// Configure and create a new <see cref="ISerializationEngine"/>.
        /// </summary>
        /// <returns>The <see cref="ITypeConfiguration"/> that used to configure and create a new <see cref="ISerializationEngine"/></returns>
        /// <remarks>
        /// See the example on <see cref="PduSerializer"/> class for usage example.
        /// </remarks>
        /// <seealso cref="PduSerializer"/>
        public static ITypeConfiguration Configure()
        {
            return new ConfigurationStore();
        }
    }
}