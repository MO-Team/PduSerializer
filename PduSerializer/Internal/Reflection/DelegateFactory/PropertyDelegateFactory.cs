using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace PduSerializer.Internal.Reflection.DelegateFactory
{
    internal delegate object LateBoundPropertyGet(object target);

    internal delegate void LateBoundPropertySet(object target, object value);

    internal class PropertyDelegateFactory : MemberDelegateFactory
    {
        protected override Type SetDelegateType
        {
            get { return typeof (LateBoundPropertySet); }
        }

        protected override Type GetDelegateType
        {
            get { return typeof (LateBoundPropertyGet); }
        }

        internal override MemberExpression CreateMemberExpression(Expression expression, MemberInfo memberInfo)
        {
            return Expression.Property(expression, (PropertyInfo) memberInfo);
        }

        protected override void EmitILSet(MemberInfo memberInfo, ILGenerator gen)
        {
            var property = (PropertyInfo) memberInfo;
            var setter = property.GetSetMethod(true);

            gen.Emit(OpCodes.Unbox_Any, property.PropertyType); // Unbox the value to its proper value type
            gen.Emit(OpCodes.Callvirt, setter); // Call the setter method
        }
    }
}