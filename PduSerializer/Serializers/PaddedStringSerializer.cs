using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using  PduSerializer.Properties;

namespace PduSerializer.Serializers
{
    internal class PaddedStringSerializer : ICustomSerializer
    {
        #region Fields

        private readonly Alignment _align;
        private readonly Encoding _encoding;
        private readonly char _paddingChar;
        private readonly int _size;

        #endregion

        #region Ctor

        public PaddedStringSerializer(int size, Encoding encoding, Alignment align = Alignment.Right,
                                      char paddingChar = ' ')
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", Messages.PaddingStringSizeOutOfRangeErrorMessage);

            if (encoding == null)
                throw new ArgumentNullException("encoding");

            _encoding = encoding;
            _size = size;
            _paddingChar = paddingChar;
            _align = align;
        }

        #endregion

        #region ICustomSerializer Implementations

        public void Serialize(object obj, BinaryWriter writer, ISerializationContext context)
        {
            var str = obj as string ?? string.Empty;
            if (str.Length > _size)
            {
                throw new SerializationException(String.Format(Messages.TooLongStringErrorMessage, _size));
            }
            if (str.Length < _size)
            {
                str = PadStringToSize(str);
            }
            writer.Write(_encoding.GetBytes(str));
        }

        public object Deserialize(BinaryReader reader, ISerializationContext context)
        {
            var stringBuffer = reader.ReadBytes(_size);
            var treamedString = _encoding.GetString(stringBuffer);

            treamedString = TreamByAlignment(treamedString);

            return treamedString;
        }

        #endregion

        #region Private Methods

        private string PadStringToSize(string str)
        {
            return _align == Alignment.Left
                       ? str.PadRight(_size, _paddingChar)
                       : str.PadLeft(_size, _paddingChar);
        }

        private string TreamByAlignment(string str)
        {
            var paddedChar = new[] {_paddingChar};

            return _align == Alignment.Right
                       ? str.TrimStart(paddedChar)
                       : str.TrimEnd(paddedChar);
        }

        #endregion
    }
}