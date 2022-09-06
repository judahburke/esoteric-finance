using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using System.Collections;
using System.Collections.Generic;

namespace Esoteric.Finance.Abstractions.DataTransfer
{
    public class CommonNamedEntityResponse : CommonNamedEntityModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<TransactionResponse>? Transactions { get; set; }
    }
}
