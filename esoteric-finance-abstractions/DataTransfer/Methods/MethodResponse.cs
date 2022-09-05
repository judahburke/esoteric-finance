using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using System.Collections.Generic;

namespace Esoteric.Finance.Abstractions.DataTransfer.Methods
{
    public class MethodResponse : MethodModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<TransactionResponse> Transactions { get; set; }
        public virtual IEnumerable<TransactionMethodResponse> TransactionMethods { get; set; }
    }
}
