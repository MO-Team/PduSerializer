using System.Reflection;
using  PduSerializer.Internal.Reflection.DelegateFactory;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal class PropertyAccessor : PropertyGetter, IMemberAccessor
    {
        private readonly bool _hasSetter;
        private readonly LateBoundPropertySet _lateBoundPropertySet;

        public PropertyAccessor(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            var method = propertyInfo.GetSetMethod(true);
            _hasSetter = method != null;
            if (_hasSetter)
            {
                _lateBoundPropertySet = (LateBoundPropertySet) PropertyDelegateFactory.CreateSet(propertyInfo);
            }
        }

        #region IMemberAccessor Members

        public void SetValue(object destination, object value)
        {
            _lateBoundPropertySet(destination, value);
        }

        #endregion
    }
}