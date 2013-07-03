using System.Reflection;
using PduSerializer.Internal.Reflection.DelegateFactory;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal class FieldAccessor : FieldGetter, IMemberAccessor
    {
        private readonly LateBoundFieldSet _lateBoundFieldSet;

        public FieldAccessor(FieldInfo fieldInfo)
            : base(fieldInfo)
        {
            _lateBoundFieldSet = (LateBoundFieldSet) FieldDelegateFactory.CreateSet(fieldInfo);
        }

        #region IMemberAccessor Members

        public void SetValue(object destination, object value)
        {
            _lateBoundFieldSet(destination, value);
        }

        #endregion
    }
}