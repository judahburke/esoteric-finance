namespace Esoteric.Finance.Abstractions.DataTransfer.Details
{
    public class TransactionDetailRequest : DetailRequest
    {
        public virtual float Multiplier { get; set; }
        public virtual long? TransactionId { get; set; }
    }
}
