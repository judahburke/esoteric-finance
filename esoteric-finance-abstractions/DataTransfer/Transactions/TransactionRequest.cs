using Esoteric.Finance.Abstractions.DataTransfer.Recipients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionRequest
        : TransactionModel<TransactionRecipientRequest, TransactionMethodRequest, TransactionCategoryRequest>
    {
        public long? TransactionId { get; set; }
    }
}
