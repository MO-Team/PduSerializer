using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using PduSerializer.Internal;
using PduSerializer.Internal.Reflection;
using PduSerializer.Properties;

namespace PduSerializer
{
    internal class SerializationEngine : ISerializationEngine
    {
        /// <summary>
        /// Constructs a new serialization engine from a configuration store
        /// </summary>
        /// <param name="configuration">configuration store to load configuration from</param>
        /// <remarks>this operation will seal the configuration store.</remarks>
        public SerializationEngine(ConfigurationStore configuration)
        {
            ConfigurationStore = configuration;
            ConfigurationStore.Seal();
        }

        internal ConfigurationStore ConfigurationStore { get; set; }

        #region ISerializationEngine Members

        /// <summary>
        /// Serializes and object into a stream
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <param name="stream">byte stream to serialize into. the stream will be closed after serialization</param>
        public void Serialize(object obj, Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                Serialize(obj, writer);
            }
        }

        /// <summary>
        /// Serializes and object into a binary writer
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <param name="writer">binary writer to serialize into. the stream will be closed after serialization</param>
        public void Serialize(object obj, BinaryWriter writer)
        {
            Type objectType = obj.GetType();

            if (ConfigurationStore.TypeDefaultSerializers.Keys.Contains(objectType))
            {
                var defaultSerializer = ConfigurationStore.TypeDefaultSerializers[objectType];

                defaultSerializer.Serialize(obj,writer, new FieldSerializationContext(new FieldSerializationInfo(), this, obj));
                return;
            }

            TypeSerializationInfo typeSerializationInfo = GetTypeSerializationInfo(objectType);

            WriteObject(obj, typeSerializationInfo, writer);
        }

        /// <summary>
        /// Deserailizes an object from a stream
        /// </summary>
        /// <param name="objectType">Type of object to be deserialized</param>
        /// <param name="stream">Stream to deserialize from</param>
        /// <returns>Deserialized object</returns>
        public object Deserialize(Type objectType, Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                return Deserialize(objectType, reader);
            }
        }

        /// <summary>
        /// Deserailizes an object from a binary reader
        /// </summary>
        /// <param name="objectType">Type of object to be deserialized</param>
        /// <param name="reader">Binary reader to deserialize from</param>
        /// <returns>Deserialized object</returns>
        public object Deserialize(Type objectType, BinaryReader reader)
        {
            return ReadObject(objectType, reader);
        }

        /// <summary>
        /// Deserailizes an object from a stream
        /// </summary>
        /// <typeparam name="T">Type of object to be deserialized</typeparam>
        /// <param name="stream">Stream to deserialize from</param>
        /// <returns>Deserialized object</returns>
        public T Deserialize<T>(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                return (T)ReadObject(typeof(T), reader);
            }
        }

        /// <summary>
        /// Deserailizes an object from a binary reader
        /// </summary>
        /// <typeparam name="T">Type of object to be deserialized</typeparam>
        /// <param name="reader">Binary reader to deserialize from</param>
        /// <returns>Deserialized object</returns>
        public T Deserialize<T>(BinaryReader reader)
        {
            return (T)ReadObject(typeof(T), reader);
        }

        #endregion

        private void WriteObject(object obj, TypeSerializationInfo typeSerializationInfo, BinaryWriter writer)
        {
            foreach (FieldSerializationInfo fieldSerializationInfo in typeSerializationInfo.SerializationFields)
            {
                var fieldValue = fieldSerializationInfo.MemberAccessor.GetValue(obj);

                if (HasCustomSerializer(fieldSerializationInfo))
                {
                    fieldSerializationInfo.CustomSerializer.Serialize(fieldValue, writer, new FieldSerializationContext(fieldSerializationInfo, this, obj));
                }
                else if (HasTypeSerializationInfo(fieldSerializationInfo.MemberType))
                {
                    WriteObject(fieldValue, ConfigurationStore.TypeSerializationInfo[fieldSerializationInfo.MemberType],
                                writer);
                }
                else
                {
                    var serializer = GetDefaultSerializerForType(fieldSerializationInfo);
                    serializer.Serialize(fieldValue, writer, new FieldSerializationContext(fieldSerializationInfo, this, obj));
                }
            }
        }

        private object ReadObject(Type objectType, BinaryReader reader)
        {
            var message = ObjectFactory.CreateObject(objectType);

            if(ConfigurationStore.TypeDefaultSerializers.Keys.Contains(objectType))
            {
                var defaultSerializer = ConfigurationStore.TypeDefaultSerializers[objectType];

                return defaultSerializer.Deserialize(reader, new FieldSerializationContext(new FieldSerializationInfo(), this, message));
            }

            var typeSerializationInfo = GetTypeSerializationInfo(objectType);

            foreach (var fieldSerializationInfo in typeSerializationInfo.SerializationFields)
            {
                object deserializedValue = ReadField(reader, fieldSerializationInfo, message);
                fieldSerializationInfo.MemberAccessor.SetValue(message, deserializedValue);
            }
            return message;
        }

        private object ReadField(BinaryReader reader, FieldSerializationInfo fieldSerializationInfo, object obj)
        {
            if (fieldSerializationInfo.CustomSerializer != null)
            {
                return fieldSerializationInfo.CustomSerializer.Deserialize(reader, new FieldSerializationContext(fieldSerializationInfo, this, obj));
            }
            if (HasTypeSerializationInfo(fieldSerializationInfo.MemberType))
            {
                return ReadObject(fieldSerializationInfo.MemberType, reader);
            }
            var serializer = GetDefaultSerializerForType(fieldSerializationInfo);
            return serializer.Deserialize(reader, new FieldSerializationContext(fieldSerializationInfo, this, obj));
        }

        private static bool HasCustomSerializer(FieldSerializationInfo fieldSerializationInfo)
        {
            return fieldSerializationInfo.CustomSerializer != null;
        }

        private bool HasTypeSerializationInfo(Type type)
        {
            return ConfigurationStore.TypeSerializationInfo.ContainsKey(type);
        }

        private TypeSerializationInfo GetTypeSerializationInfo(Type objectType)
        {
            if (!HasTypeSerializationInfo(objectType))
            {
                throw new SerializationException(String.Format(Messages.NoDescriptorForTypeErrorMessage,
                                                               objectType.FullName));
            }
            return ConfigurationStore.TypeSerializationInfo[objectType];
        }

        private ICustomSerializer GetDefaultSerializerForType(FieldSerializationInfo fieldSerializationInfo)
        {
            var memberType = fieldSerializationInfo.MemberType;
            if (ConfigurationStore.TypeDefaultSerializers.ContainsKey(memberType))
                return ConfigurationStore.TypeDefaultSerializers[memberType];

            var customSerializer = GetCustomSerializer(memberType);
            if (customSerializer != null)
                return customSerializer;

            throw new SerializationException(string.Format(Messages.NotSerializableTypeErrorMessage,
                                                           fieldSerializationInfo.FieldInfo.Name, memberType));
        }

        private static ICustomSerializer GetCustomSerializer(MemberInfo field)
        {
            return field.GetCustomAttributes(typeof (CustomSerializerAttribute), true)
                .Cast<CustomSerializerAttribute>()
                .Select(x => x.CustomSerializer)
                .FirstOrDefault();
        }
    }
}