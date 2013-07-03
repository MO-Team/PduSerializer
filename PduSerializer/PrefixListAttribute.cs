namespace PduSerializer
{
    public class PrefixListAttribute : CustomSerializerAttribute
    {
        public PrefixListAttribute()
        {
            CustomSerializer = new PrefixListSerializer();
        }

    }
}