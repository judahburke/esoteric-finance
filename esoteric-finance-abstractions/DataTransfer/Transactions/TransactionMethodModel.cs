using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionMethodModel
    {
        public string Method { get; set; }
        public decimal Amount { get; set; }
    }
}
