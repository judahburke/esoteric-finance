using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// many-to-many between <see cref="Payment.Transaction"/> and <see cref="Payment.SubCategory"/>
    /// </summary>
    public class TransactionSubCategory : CommonAuditedEntity
    {
        [Key]
        public virtual long TransactionSubCategoryId { get; set; }
        public virtual long TransactionId { get; set; }
        public virtual int SubCategoryId { get; set; }
        public virtual decimal Multiplier { get; set; }

        public virtual Transaction Transaction { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }
}
