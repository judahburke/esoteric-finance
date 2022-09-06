namespace Esoteric.Finance.Abstractions.DataTransfer.Details
{
    public class DetailRequest : DetailModel
    {
        public virtual long? Id { get; set; }
        public virtual CommonNamedEntityRequest Category { get; set; }
    }
}
