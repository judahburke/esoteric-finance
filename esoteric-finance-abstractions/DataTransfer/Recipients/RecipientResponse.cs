using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using System.Collections.Generic;

namespace Esoteric.Finance.Abstractions.DataTransfer.Recipients
{
    public class RecipientResponse : RecipientModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<TransactionResponse> Transactions { get; set; }
    }
}
