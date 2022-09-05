using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionMethodRequest : TransactionMethodModel
    {
        public int? MethodId { get; set; }
    }
}
