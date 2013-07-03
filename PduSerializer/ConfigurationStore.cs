using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using PduSerializer.Internal;
using PduSerializer.Internal.Reflection.MemberAccessor;
using PduSerializer.Properties;
using PduSerializer.Serializers;

namespace PduSerializer
{
    public class ConfigurationStore : ISerializationConfiguration
    {
        private bool _isSealed;

        public ConfigurationStore()
        {
            TypeSerializationInfo = new Dictionary<Type, TypeSerializationInfo>();
            TypeDefaultSerializers = CreateDefaultSerializers();
        }

        #region Properties

        internal readonly Dictionary<Type, TypeSerializationInfo> TypeSerializationInfo;
        internal readonly Dictionary<Type, ICustomSerializer> TypeDefaultSerializers;

        #endregion

        #region ISerializationConfiguration Members

        /// <summary>
        /// register types from a specified assembly
        /// </summary>
        /// <typeparam name="T">type in the specified assembly</typeparam>
        /// <returns></returns>
        public ISerializationConfiguration AddTypesFromAssemblyOf<T>()
        {
            Type typeInAssemly = typeof (T);
            IEnumerable<Type> types = from t in typeInAssemly.Assembly.GetTypes()
                                      where HasPduMessageAttribute(t) && (t.IsClass || t.IsValueType) &&
                                            !t.IsAbstract && t.IsPublic
                                      select t;

            foreach (Type type in types)
            {
                AddType(type);
            }
            return this;
        }

        public ISerializationConfiguration AddTypesFromAssembly(Assembly assembly)
        {
            IEnumerable<Type> types = from t in assembly.GetTypes()
                                      where HasPduMessageAttribute(t) && (t.IsClass || t.IsValueType) &&
                                            !t.IsAbstract && t.IsPublic
                                      select t;

            foreach (Type type in types)
            {
                AddType(type);
            }
            return this;
        }

        /// <summary>
        /// register a specified type
        /// </summary>
        /// <param name="type">type to register</param>
        /// <returns></returns>
        public ISerializationConfiguration AddType(Type type)
        {
            VerifyInstanceNotSealed();
            if (TypeSerializationInfo.ContainsKey(type))
                return this;

            var pduMessageAttribute = type.GetCustomAttribute<PduMessageAttribute>();
            if (pduMessageAttribute == null)
            {
                throw new SerializationException(string.Format(Messages.MissingPduMessageAttributeErrorMessage,
                                                               type.Name));
            }
            TypeSerializationInfo typeSerializationInfo = CreateSerializationInfoForType(type,
                                                                                         pduMessageAttribute.
                                                                                             ChangeByteOrder);
            TypeSerializationInfo.Add(type, typeSerializationInfo);
            return this;
        }

        /// <summary>
        /// register a specified type
        /// </summary>
        /// <typeparam name="T">type to register</typeparam>
        /// <returns></returns>
        public ISerializationConfiguration AddType<T>()
        {
            return AddType(typeof (T));
        }

        /// <summary>
        /// Register a custom serializer as default serializer for a given type.
        /// </summary>
        /// <param name="type">Serialization type</param>
        /// <param name="serializer">serializer</param>
        /// <returns></returns>
        public ISerializationConfiguration AddSerializer(Type type, ICustomSerializer serializer)
        {
            TypeDefaultSerializers[type] = serializer;
            return this;
        }

        /// <summary>
        /// Seals the configuration store, and creates a new serialization engine based on this store.
        /// </summary>
        /// <returns></returns>
        public ISerializationEngine CreateSerializationEngine()
        {
            return new SerializationEngine(this);
        }

        #endregion

        #region Private Methods

        private TypeSerializationInfo CreateSerializationInfoForType(Type type, bool changeByteOrder)
        {
            var typeSerializationInfo = new TypeSerializationInfo();
            var fields = GetSerializableMembers(type);

            foreach (MemberInfo field in fields)
            {
                var fieldAttribute = GetFieldAttribute(field);

                var serialization = CreateSerializationInfoFromMemberInfo(changeByteOrder, field,
                                                                                             fieldAttribute);
                if (IsComplexField(field.GetMemberType()))
                {
                    AddType(field.GetMemberType());
                }
                serialization.CustomSerializer = GetCustomSerializer(field);
                VerifyUniqueFieldOrder(typeSerializationInfo, field, serialization);
                typeSerializationInfo.SerializationFields.Add(serialization);
            }

            typeSerializationInfo.SerializationFields =
                typeSerializationInfo.SerializationFields.OrderBy(x => x.Position).ToList();
            return typeSerializationInfo;
        }

        private static void VerifyUniqueFieldOrder(TypeSerializationInfo typeSerializationInfo, MemberInfo field,
                                                   FieldSerializationInfo serialization)
        {
            if (typeSerializationInfo.SerializationFields.Exists(x => x.Position == serialization.Position))
            {
                throw new SerializationException(string.Format(Messages.DuplicateFieldOrderErrorMessage, field.Name));
            }
        }

        private static FieldSerializationInfo CreateSerializationInfoFromMemberInfo(
            bool changeOrder, MemberInfo field, FieldAttribute fieldAttribute)
        {
            var customSerializer = GetCustomSerializer(field);
            var serialization = new FieldSerializationInfo
                                    {
                                        CustomSerializer = customSerializer,
                                        Position = fieldAttribute.Position,
                                        FieldInfo = field,
                                        ChangeByteOrder = changeOrder,
                                        MemberType = field.GetMemberType(),
                                        MemberAccessor = field.ToMemberAccessor()
                                    };
            return serialization;
        }

        private static IEnumerable<MemberInfo> GetSerializableMembers(Type type)
        {
            return type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => GetFieldAttribute(f) != null);
        }

        private void VerifyInstanceNotSealed()
        {
            if (_isSealed)
            {
                throw new InvalidOperationException(Messages.SealedConfigurationErrorMessage);
            }
        }

        private static bool HasPduMessageAttribute(Type type)
        {
            return type.GetCustomAttribute<PduMessageAttribute>() != null;
        }

        private static FieldAttribute GetFieldAttribute(MemberInfo field)
        {
            var fieldAttributes =
                field.GetCustomAttributes(typeof (FieldAttribute), false).Cast<FieldAttribute>();
            var fieldAttribute = fieldAttributes.FirstOrDefault();
            return fieldAttribute;
        }

        private static bool IsComplexField(Type fieldType)
        {
            return fieldType.GetCustomAttribute<PduMessageAttribute>() != null;
        }

        private static ICustomSerializer GetCustomSerializer(MemberInfo field)
        {
            var customSerializerAttributes = field.GetCustomAttribute<CustomSerializerAttribute>();
            return customSerializerAttributes == null ? null : customSerializerAttributes.CustomSerializer;
        }

        private static Dictionary<Type, ICustomSerializer> CreateDefaultSerializers()
        {
            return new Dictionary<Type, ICustomSerializer>
                       {
                           {typeof (Int16), new Int16Serializer()},
                           {typeof (Int32), new Int32Serializer()},
                           {typeof (Int64), new Int64Serializer()},
                           {typeof (UInt16), new UInt16Serializer()},
                           {typeof (UInt32), new UInt32Serializer()},
                           {typeof (UInt64), new UInt64Serializer()},
                           {typeof (float), new FloatSerializer()},
                           {typeof (String), new StringSerializer()},
                           {typeof (Boolean), new BooleanSerializer()},
                           {typeof (Byte), new ByteSerializer()},
                           {typeof (SByte), new SByteSerializer()},
                           {typeof (Double), new DoubleSerializer()}
                       };
        }

        #endregion

        internal void Seal()
        {
            _isSealed = true;
        }
    }
}