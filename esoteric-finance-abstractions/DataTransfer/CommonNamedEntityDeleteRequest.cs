namespace Esoteric.Finance.Abstractions.DataTransfer
{
    public class CommonNamedEntityDeleteRequest : CommonNamedEntityRequest
    {
        public virtual string ReplacementName { get; set; }
    }
}
