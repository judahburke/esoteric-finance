using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using System.Collections.Generic;

namespace Esoteric.Finance.Abstractions.DataTransfer.Categories
{
    public class SubCategoryResponse : SubCategoryModel
    {
        public virtual int Id { get; set; }
        public virtual CategoryResponse Category { get; set; }
        public virtual IEnumerable<TransactionResponse> Transactions { get; set; }
        public virtual IEnumerable<TransactionCategoryResponse> TransactionCategories { get; set; }
    }
}
