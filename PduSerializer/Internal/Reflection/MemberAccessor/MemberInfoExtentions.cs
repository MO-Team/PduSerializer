using System;
using System.Linq;
using System.Reflection;
using  PduSerializer.Properties;
using System.Linq.Expressions;
using  PduSerializer.Internal.Reflection.DelegateFactory;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal static class MemberInfoExtentions
    {
        internal static Type GetMemberType(this MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo) memberInfo).PropertyType;
            if (memberInfo is FieldInfo)
                return ((FieldInfo) memberInfo).FieldType;
            throw new ArgumentException(Messages.UnsupportedMemberType);
        }

        internal static IMemberAccessor ToMemberAccessor(this MemberInfo accessorCandidate)
        {
            var fieldInfo = accessorCandidate as FieldInfo;
            if (fieldInfo != null)
                return accessorCandidate.DeclaringType.IsValueType
                           ? (IMemberAccessor) new ValueTypeFieldAccessor(fieldInfo)
                           : new FieldAccessor(fieldInfo);

            var propertyInfo = accessorCandidate as PropertyInfo;
            if (propertyInfo != null)
                return accessorCandidate.DeclaringType.IsValueType
                           ? (IMemberAccessor) new ValueTypePropertyAccessor(propertyInfo)
                           : new PropertyAccessor(propertyInfo);

            throw new ArgumentException(Messages.UnsupportedMemberType);
        }

        internal static T GetCustomAttribute<T>(this Type type, bool inherit = false)
        {
            return type.GetCustomAttributes(typeof (T), inherit).Cast<T>().FirstOrDefault();
        }

        internal static T GetCustomAttribute<T>(this MemberInfo type, bool inherit = false)
        {
            return type.GetCustomAttributes(typeof (T), inherit).Cast<T>().FirstOrDefault();
        }

        internal static IMemberGetter GetMemberGetter(this Type type, string memberPath)
        {
            return new NestedMemberGetter(type, memberPath);
        }
    }
}