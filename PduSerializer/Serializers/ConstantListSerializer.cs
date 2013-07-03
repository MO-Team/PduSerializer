using System;
using System.Collections;
using System.IO;
using  PduSerializer.Internal;
using PduSerializer.Properties;

namespace PduSerializer.Serializers
{
    internal class ConstantListSerializer : ListSerializer
    {
        public ConstantListSerializer(int numberOfItems)
        {
            if (numberOfItems <= 0)
                throw new ArgumentOutOfRangeException("numberOfItems", Messages.ListSizeOutOfRangeErrorMessage);

            _numberOfItems = numberOfItems;
        }

        private int _numberOfItems;

        protected override int GetListSize(ISerializationContext context)
        {
            return _numberOfItems;
        }
    }
}
