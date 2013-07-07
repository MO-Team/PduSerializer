using System;
using System.IO;
using System.Net;

namespace PduSerializer.Serializers
{
    internal class EnumSerializer : ICustomSerializer
    {
        #region ISerializer Members

        public void Serialize(object obj, BinaryWriter stream, ISerializationContext context)
        {
            var intObj = (Int32) obj;
            if (context.ChangeByteOrder)
                stream.Write(IPAddress.HostToNetworkOrder(intObj));
            else
                stream.Write(intObj);
        }

        public object Deserialize(BinaryReader stream, ISerializationContext context)
        {
            if (context.ChangeByteOrder)
                return IPAddress.NetworkToHostOrder(stream.ReadInt32());
            return stream.ReadInt32();
        }

        #endregion
    }
}