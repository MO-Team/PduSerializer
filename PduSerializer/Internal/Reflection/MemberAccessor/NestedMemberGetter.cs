using PduSerializer.Internal.Reflection.DelegateFactory;
using PduSerializer.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal class NestedMemberGetter : IMemberGetter
    {
        private readonly Func<object, object> _lateBoundPropertyGet;

        internal NestedMemberGetter(Type containerType, string memberPath)
        {
            var instanceParameter = Expression.Parameter(typeof(object), "container");

            var memberExpression = CreateNestedMembersExpression(Expression.Convert(instanceParameter, containerType), 
                                                                 memberPath.Split('.'));

            var lambda = Expression.Lambda(typeof(Func<object, object>),
                                           Expression.Convert(memberExpression, typeof(object)),
                                           instanceParameter);

            _lateBoundPropertyGet = (Func<object, object>) lambda.Compile();
        }

        private Expression CreateNestedMembersExpression(Expression instanceExpression, ICollection<string> nestedMembers)
        {
            var propertyDelegateFactory = new PropertyDelegateFactory();
            var fieldDelegateFactory = new FieldDelegateFactory();
            
            var memberExpression = instanceExpression;

            foreach (var member in nestedMembers)
            {
                var memberInfo = memberExpression.Type.GetMember(member).FirstOrDefault();
                if (memberInfo == null)
                    throw new TargetException(String.Format(Messages.MemberNotFound, member, memberExpression.Type));

                if (memberInfo is PropertyInfo)
                    memberExpression = propertyDelegateFactory.CreateMemberExpression(memberExpression, memberInfo);
                if (memberInfo is FieldInfo)
                    memberExpression = fieldDelegateFactory.CreateMemberExpression(memberExpression, memberInfo);
            }

            return memberExpression;
        }

        public object GetValue(object source)
        {
            return _lateBoundPropertyGet(source);
        }
    }
}
