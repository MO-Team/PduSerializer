using System;

namespace PduSerializer
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class PduMessageAttribute : Attribute
    {
        public bool ChangeByteOrder { get; set; }
    }
}