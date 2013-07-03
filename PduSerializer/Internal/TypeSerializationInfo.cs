using System.Collections.Generic;

namespace PduSerializer.Internal
{
    internal class TypeSerializationInfo
    {
        public List<FieldSerializationInfo> SerializationFields;

        public TypeSerializationInfo()
        {
            SerializationFields = new List<FieldSerializationInfo>();
        }
    }
}