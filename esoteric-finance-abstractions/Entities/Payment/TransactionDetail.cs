using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// many-to-many entity between <see cref="Payment.Transaction"/> and <see cref="Payment.Detail"/>
    /// </summary>
    public class TransactionDetail : CommonAuditedEntity
    {
        [Key]
        public virtual long TransactionDetailId { get; set; }
        [Required]
        public virtual long TransactionId { get; set; }
        [Required]
        public virtual long DetailId { get; set; }
        [Required]
        public virtual float Multiplier { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual Detail Detail { get; set; }
    }
}
