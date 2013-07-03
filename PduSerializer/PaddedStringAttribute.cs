using System.Text;
using  PduSerializer.Serializers;

namespace PduSerializer
{
    /// <summary>
    /// Used to determinates the padding direction
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Hebrew
        /// </summary>
        Right,

        /// <summary>
        /// English
        /// </summary>
        Left
    }

    /// <summary>
    /// Use this attribute on top of a hebrew string field in case of a pre-defined size string
    /// </summary>
    public class PaddedStringAttribute : CustomSerializerAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">Size of string block</param>
        /// <param name="align">Used to determinates the padding direction. Default is right</param>
        /// <param name="padding">The char that fills the missing spaces. Default is ' '</param>
        public PaddedStringAttribute(int size, Alignment align = Alignment.Right, char padding = ' ')
        {
            CustomSerializer = new PaddedStringSerializer(size, Encoding.GetEncoding(1255), paddingChar : padding,
                                                          align : align);
        }
    }
}