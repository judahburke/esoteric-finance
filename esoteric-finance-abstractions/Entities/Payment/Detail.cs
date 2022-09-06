using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// one-to-many with <see cref="Payment.TransactionDetail"/>
    /// many-to-one with <see cref="Payment.Category"/>
    /// </summary>
    public class Detail : CommonAuditedEntity
    {
        [Key]
        public virtual long DetailId { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
        public virtual Category Category { get; set; }
    }
}
