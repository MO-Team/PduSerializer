using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  PduSerializer.Serializers;

namespace PduSerializer
{
    /// <summary>
    /// Defines the field as an array or list field with variable size
    /// </summary>
    public class VariableListAttribute : CustomSerializerAttribute
    {
        /// <summary>
        /// Defines the field as an array or list field with variable size
        /// </summary>
        /// <param name="listSizeMemberName">The name of the member that holds the list size, or a path to it</param>
        /// <example>[VariableList("Header.NumOfIMyClass", typeof(MyClass[]))]</example>
        public VariableListAttribute(string listSizeMemberName)
        {
            CustomSerializer = new VariableListSerializer(listSizeMemberName);
        }
    }
}
