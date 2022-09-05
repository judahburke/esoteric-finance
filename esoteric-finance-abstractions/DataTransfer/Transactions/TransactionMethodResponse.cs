using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionMethodResponse : TransactionMethodModel
    {
        public int MethodId { get; set; }
        public long TransactionId { get; set; }
    }
}
