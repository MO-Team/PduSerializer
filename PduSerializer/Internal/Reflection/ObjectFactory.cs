using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PduSerializer.Internal.Reflection
{
    internal delegate object LateBoundCtor();

    internal static class ObjectFactory
    {
        #region Ctor Cache

        private static readonly object CtorCacheSync = new object();
        private static readonly Dictionary<Type, LateBoundCtor> CtorCache = new Dictionary<Type, LateBoundCtor>();

        #endregion

        internal static object CreateObject(Type type)
        {
            return CreateCtor(type)();
        }

        private static LateBoundCtor CreateCtor(Type type)
        {
            LateBoundCtor ctor;
            if (!CtorCache.TryGetValue(type, out ctor))
            {
                lock (CtorCacheSync)
                {
                    if (CtorCache.TryGetValue(type, out ctor))
                    {
                        return ctor;
                    }

                    var ctorExpression =
                        Expression.Lambda<LateBoundCtor>(Expression.Convert(Expression.New(type), typeof (object)));
                    ctor = ctorExpression.Compile();
                    CtorCache.Add(type, ctor);
                }
            }
            return ctor;
        }
    }
}