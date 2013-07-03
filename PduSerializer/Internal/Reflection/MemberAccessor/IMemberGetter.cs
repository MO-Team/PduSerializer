namespace PduSerializer.Internal.Reflection.MemberAccessor
{
    internal interface IMemberGetter
    {
        object GetValue(object source);
    }
}