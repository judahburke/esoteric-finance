using Esoteric.Finance.Abstractions.DataTransfer.Categories;
using System.Collections.Generic;

namespace Esoteric.Finance.Abstractions.DataTransfer.Details
{
    public class DetailResponse : DetailModel
    {
        public virtual long Id { get; set; }
        public virtual CategoryResponse Category { get; set; }
        public virtual IEnumerable<TransactionDetailResponse>? Transactions { get; set; }
    }
}
