namespace Esoteric.Finance.Abstractions.DataTransfer
{
    public class CommonNamedEntityDeleteRequest
    {
        public virtual string Name { get; set; }
        public virtual string ReplacementName { get; set; }

    }
}
