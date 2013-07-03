using System.Reflection;

namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal class ValueTypeFieldAccessor : FieldGetter, IMemberAccessor
    {
        private readonly FieldInfo _fieldInfo;

        public ValueTypeFieldAccessor(FieldInfo fieldInfo)
            : base(fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        #region IMemberAccessor Members

        public void SetValue(object destination, object value)
        {
            _fieldInfo.SetValue(destination, value);
        }

        #endregion
    }
}