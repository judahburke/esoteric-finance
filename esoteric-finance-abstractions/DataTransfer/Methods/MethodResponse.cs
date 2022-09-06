using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Methods
{
    public class MethodResponse : CommonNamedEntityModel
    {
        public virtual int Id { get; set; }
        public IEnumerable<TransactionMethodResponse>? Transactions { get; set; }
    }
}
