using PduSerializer.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PduSerializer.Serializers
{
    internal abstract class ListSerializer : ICustomSerializer
    {
        private static readonly string GenericListTypeName;

        static ListSerializer()
        {
            GenericListTypeName = typeof(ICollection<>).Name;
        }

        #region ICustomSerializer Members

        public virtual void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var list = (ICollection)obj;
            var expectedListSize = GetListSize(context);
            if (expectedListSize != list.Count)
                throw new ArgumentOutOfRangeException("obj", String.Format(Messages.DeclaredListSizeIsDiffrentThanActualSize, expectedListSize, list.Count));

            foreach (var item in list)
            {
                context.SerializationEngine.Serialize(item, stream);
            }
        }

        public virtual object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            var listSize = GetListSize(context);
            var elementType = GetElementType(context.MemberType);
            var list = CreateListInstance(context.MemberType, listSize, elementType);

            for (var i = 0; i < listSize; i++)
            {
                list[i] = context.SerializationEngine.Deserialize(elementType, stream);
            }
            return list;
        }

        #endregion

        protected abstract int GetListSize(ISerializationContext context);

        protected virtual Type GetElementType(Type listType)
        {
            var listInterface = listType.GetInterface(GenericListTypeName);
            if (listInterface != null)
                return listInterface.GetGenericArguments().First();

            if (listType.IsArray)
                return listType.GetElementType();

            throw new ArgumentException(String.Format(Messages.UnsupportedListType, listType));
        }

        protected virtual IList CreateListInstance(Type listType, int listSize, Type elementType)
        {
            var list = Array.CreateInstance(elementType, listSize);

            if (listType.IsInterface ||  // simple array implements IList<T> and ICollection<T>
                listType.BaseType.Equals(typeof(Array)))
                return list;
            
            if (listType.IsGenericType && listType.GetGenericTypeDefinition().Equals(typeof(List<>)))
                return (IList)Activator.CreateInstance(listType, list);

            throw new ArgumentException(String.Format(Messages.UnsupportedListType, listType));
        }
    }
}
