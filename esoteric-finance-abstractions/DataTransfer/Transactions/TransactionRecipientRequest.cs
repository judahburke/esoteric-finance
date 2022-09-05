using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionRecipientRequest : TransactionRecipientModel
    {
        public int? RecipientId { get; set; }
    }
}
