using Esoteric.Finance.Abstractions.DataTransfer.Recipients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionModel<TRecipient, TMethod, TCategory>
        where TRecipient: TransactionRecipientModel
        where TMethod : TransactionMethodModel
        where TCategory : TransactionCategoryModel
    {
        public DateTimeOffset TransactionDate { get; set; }
        public DateTimeOffset? PostedDate { get; set; }
        public TRecipient Recipient { get; set; }
        public IEnumerable<TMethod> Methods { get; set; }
        public IEnumerable<TCategory> Categories { get; set; }
    }
}
