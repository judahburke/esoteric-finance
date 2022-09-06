using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Methods
{
    public class TransactionMethodRequest : CommonNamedEntityModel
    {
        public int? Id { get; set; }
        public decimal Amount { get; set; }
        public long? TransactionId { get; set; }
    }
}
