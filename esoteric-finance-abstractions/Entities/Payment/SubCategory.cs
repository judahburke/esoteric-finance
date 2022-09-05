using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// lookup entity for payment subcategories
    /// many-to-one with <see cref="Payment.Category"/>
    /// </summary>
    public class SubCategory : CommonNamedEntity
    {
        [Key]
        public virtual int SubCategoryId { get; set; }
        public virtual int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<TransactionSubCategory> Transactions { get; set; }
    }
}
