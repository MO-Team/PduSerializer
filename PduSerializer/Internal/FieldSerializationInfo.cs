using System;
using System.Reflection;
using PduSerializer.Internal.Reflection.MemberAccessor;

namespace PduSerializer.Internal
{
    internal class FieldSerializationInfo 
    {
        public ICustomSerializer CustomSerializer;
        public MemberInfo FieldInfo;
        public IMemberAccessor MemberAccessor;
        public Type MemberType { get; set; }


        public uint Position;

        #region ISerializationContext Members

        public bool ChangeByteOrder { get; internal set; }

        #endregion
    }

    internal class FieldSerializationContext : ISerializationContext
    {
        public FieldSerializationContext(FieldSerializationInfo fieldSerializationInfo, ISerializationEngine serializationEngine, object objectInstance)
        {
            _fieldSerializationInfo = fieldSerializationInfo;
            _objectInstance = objectInstance;
            SerializationEngine = serializationEngine;
        }

        private FieldSerializationInfo _fieldSerializationInfo;
        private object _objectInstance;
        
        #region ISerializationContext Members

        public bool ChangeByteOrder { get { return _fieldSerializationInfo.ChangeByteOrder; } }
        
        public Type MemberType { get { return _fieldSerializationInfo.MemberType; } }

        public ISerializationEngine SerializationEngine { get; private set; }

        public object GetObjectMemberValue(string propertyPath)
        {
            var memberGetter = _objectInstance.GetType().GetMemberGetter(propertyPath);
            return memberGetter.GetValue(_objectInstance);
        }

        #endregion
    }
}
