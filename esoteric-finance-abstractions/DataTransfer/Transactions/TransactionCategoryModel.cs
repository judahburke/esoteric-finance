using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{

    public class TransactionCategoryModel
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public decimal Multiplier { get; set; }
    }
}
