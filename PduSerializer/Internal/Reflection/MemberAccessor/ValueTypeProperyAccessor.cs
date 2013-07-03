using System.Reflection;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal class ValueTypePropertyAccessor : PropertyGetter, IMemberAccessor
    {
        private readonly bool _hasSetter;
        private readonly MethodInfo _methodInfo;

        public ValueTypePropertyAccessor(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            var method = propertyInfo.GetSetMethod(true);
            _hasSetter = method != null;
            if (_hasSetter)
            {
                _methodInfo = method;
            }
        }

        #region IMemberAccessor Members

        public void SetValue(object destination, object value)
        {
            _methodInfo.Invoke(destination, new[] { value });
        }

        #endregion
    }
}