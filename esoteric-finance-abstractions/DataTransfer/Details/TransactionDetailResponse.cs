using Esoteric.Finance.Abstractions.DataTransfer.Categories;

namespace Esoteric.Finance.Abstractions.DataTransfer.Details
{
    public class TransactionDetailResponse: DetailModel
    {
        public virtual long Id { get; set; }
        public virtual CategoryResponse Category { get; set; }
        public virtual float Multiplier { get; set; }
        public virtual long TransactionId { get; set; }
    }
}
