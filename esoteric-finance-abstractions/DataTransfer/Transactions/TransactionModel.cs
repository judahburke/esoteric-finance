using Esoteric.Finance.Abstractions.DataTransfer.Details;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public abstract class TransactionModel
    {
        public DateTimeOffset TransactionDate { get; set; }
        public DateTimeOffset? PostedDate { get; set; }
    }

    public class TransactionModel<TInitiator, TRecipient, TMethod, TDetail> : TransactionModel
        where TInitiator : CommonNamedEntityModel
        where TRecipient: CommonNamedEntityModel
        where TMethod : CommonNamedEntityModel
        where TDetail : DetailModel
    {
        public TInitiator Initiator { get; set; }
        public TRecipient Recipient { get; set; }
        public IEnumerable<TMethod> Methods { get; set; }
        public IEnumerable<TDetail> Details { get; set; }
    }
}
