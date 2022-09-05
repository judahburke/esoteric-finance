using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionRecipientResponse : TransactionRecipientModel
    {
        public int RecipientId { get; set; }
        public IList<long>? TransactionIds { get; set; }
    }
}
