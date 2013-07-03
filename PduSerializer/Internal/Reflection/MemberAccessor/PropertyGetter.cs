using System.Reflection;
using PduSerializer.Internal.Reflection.DelegateFactory;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal class PropertyGetter : IMemberGetter
    {
        private readonly LateBoundPropertyGet _lateBoundPropertyGet;
        private readonly PropertyInfo _propertyInfo;

        internal PropertyGetter(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            PropertyDelegateFactory = new PropertyDelegateFactory();

            if (_propertyInfo.GetGetMethod(true) != null)
                _lateBoundPropertyGet = (LateBoundPropertyGet) PropertyDelegateFactory.CreateGet(propertyInfo);
        }

        protected PropertyDelegateFactory PropertyDelegateFactory { get; private set; }

        #region IMemberGetter Members

        public object GetValue(object source)
        {
            return _lateBoundPropertyGet(source);
        }

        #endregion
    }
}