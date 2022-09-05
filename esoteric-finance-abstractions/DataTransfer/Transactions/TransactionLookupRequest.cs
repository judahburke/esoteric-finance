using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionLookupRequest
    {
        public long? TransactionId { get; set; }
        public DateTimeOffset? MinimumDate { get; set; }
        public DateTimeOffset? MaximumDate { get; set; }
    }
}
