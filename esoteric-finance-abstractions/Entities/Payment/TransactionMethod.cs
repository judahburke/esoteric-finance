using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// many-to-many entity between <see cref="Payment.Transaction"/> and <see cref="Payment.Method"/>
    /// </summary>
    public class TransactionMethod : CommonAuditedEntity
    {
        [Key]
        public virtual long TransactionMethodId { get; set; }
        public virtual long TransactionId { get; set; }
        public virtual int MethodId { get; set; }
        public virtual decimal Amount { get; set; }

        public virtual Transaction Transaction { get; set; }
        public virtual Method Method { get; set; }
    }
}
