using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PduSerializer.Serializers
{
    internal class VariableListSerializer : ListSerializer
    {
        private Func<object, int> _converter;

        public VariableListSerializer(string listSizeMemberName)
        {
            _listSizeMemberName = listSizeMemberName;
        }

        private string _listSizeMemberName;

        protected override int GetListSize(ISerializationContext context)
        {
            var value = context.GetObjectMemberValue(_listSizeMemberName);
            return ConvertToInt(value);
        }

        private int ConvertToInt(object value)
        {
            if (_converter == null)
            {
                var param = Expression.Parameter(typeof(object), "value");
                var lambda = Expression.Lambda<Func<object, int>>(Expression.Convert(Expression.Convert(param, value.GetType()),
                                                                                     typeof(int)), param);
                _converter = lambda.Compile();
            }
            return _converter(value);
        }
    }
}
