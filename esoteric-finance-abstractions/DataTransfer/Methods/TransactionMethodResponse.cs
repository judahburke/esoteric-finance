using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionMethodResponse : CommonNamedEntityModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public long TransactionId { get; set; }
    }
}
