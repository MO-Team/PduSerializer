using System.Reflection;
using PduSerializer.Internal.Reflection.DelegateFactory;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal class FieldGetter : IMemberGetter
    {
        private readonly LateBoundFieldGet _lateBoundFieldGet;

        internal FieldGetter(FieldInfo fieldInfo)
        {
            FieldDelegateFactory = new FieldDelegateFactory();
            _lateBoundFieldGet = (LateBoundFieldGet) FieldDelegateFactory.CreateGet(fieldInfo);
        }

        protected FieldDelegateFactory FieldDelegateFactory { get; private set; }

        #region IMemberGetter Members

        public object GetValue(object source)
        {
            return _lateBoundFieldGet(source);
        }

        #endregion
    }
}