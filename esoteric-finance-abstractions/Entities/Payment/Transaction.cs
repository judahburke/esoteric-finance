using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// core entity representing a transaction
    /// many-to-one with <see cref="Payment.Recipient"/>
    /// </summary>
    public class Transaction : CommonAuditedEntity
    {
        [Key]
        public virtual long TransactionId { get; set; }
        public virtual int InitiatorId { get; set; }
        public virtual int RecipientId { get; set; }
        public virtual DateTimeOffset TransactionDate { get; set; }
        public virtual DateTimeOffset? PostedDate { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual Initiator Initiator { get; set; }
        public virtual Recipient Recipient { get; set; }
        public virtual ICollection<TransactionMethod> TransactionMethods { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
