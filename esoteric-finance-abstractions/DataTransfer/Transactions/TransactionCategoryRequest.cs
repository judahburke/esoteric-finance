using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{

    public class TransactionCategoryRequest : TransactionCategoryModel
    {
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
