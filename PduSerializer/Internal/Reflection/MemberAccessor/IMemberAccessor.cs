namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal interface IMemberAccessor : IMemberGetter
    {
        void SetValue(object destination, object value);
    }
}