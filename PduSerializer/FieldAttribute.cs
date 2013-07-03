using System;

namespace PduSerializer
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FieldAttribute : Attribute
    {
        public uint Position { get; set; }
    }
}