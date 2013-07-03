using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace PduSerializer.Internal.Reflection.DelegateFactory
{
    internal abstract class MemberDelegateFactory
    {
        protected abstract Type SetDelegateType { get; }
        protected abstract Type GetDelegateType { get; }

        internal Delegate CreateSet(MemberInfo memberInfo)
        {
            var sourceType = memberInfo.DeclaringType;
            var method = CreateDynamicMethod(memberInfo, sourceType);

            var gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0); // Load input to stack
            gen.Emit(OpCodes.Castclass, sourceType); // Cast to source type
            gen.Emit(OpCodes.Ldarg_1); // Load value to stack
            EmitILSet(memberInfo, gen);
            gen.Emit(OpCodes.Ret);


            return method.CreateDelegate(SetDelegateType);
        }

        protected abstract void EmitILSet(MemberInfo memberInfo, ILGenerator gen);

        internal Delegate CreateGet(MemberInfo memberInfo)
        {
            var instanceParameter = Expression.Parameter(typeof (object), "target");

            var member =
                CreateMemberExpression(Expression.Convert(instanceParameter, memberInfo.DeclaringType), memberInfo);

            var lambda = Expression.Lambda(GetDelegateType,
                                           Expression.Convert(member, typeof (object)),
                                           instanceParameter);

            return lambda.Compile();
        }

        internal abstract MemberExpression CreateMemberExpression(Expression expression, MemberInfo memberInfo);

        private static DynamicMethod CreateDynamicMethod(MemberInfo member, Type sourceType)
        {
            if (sourceType.IsInterface)
                return new DynamicMethod("Set" + member.Name, null, new[] {typeof (object), typeof (object)},
                                         sourceType.Assembly.ManifestModule, true);

            return new DynamicMethod("Set" + member.Name, null, new[] {typeof (object), typeof (object)}, sourceType,
                                     true);
        }
    }
}