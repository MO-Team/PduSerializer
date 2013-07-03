using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace PduSerializer.Internal.Reflection.DelegateFactory
{
    internal delegate object LateBoundFieldGet(object target);

    internal delegate void LateBoundFieldSet(object target, object value);

    internal class FieldDelegateFactory : MemberDelegateFactory
    {
        protected override Type SetDelegateType
        {
            get { return typeof (LateBoundFieldSet); }
        }

        protected override Type GetDelegateType
        {
            get { return typeof (LateBoundFieldGet); }
        }

        protected override void EmitILSet(MemberInfo memberInfo, ILGenerator gen)
        {
            gen.Emit(OpCodes.Unbox_Any, ((FieldInfo) memberInfo).FieldType); // Unbox the value to its proper value type
            gen.Emit(OpCodes.Stfld, (FieldInfo) memberInfo); // Set the value to the input field
        }

        internal override MemberExpression CreateMemberExpression(Expression expression, MemberInfo memberInfo)
        {
            return Expression.Field(expression, (FieldInfo) memberInfo);
        }
    }
}