using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// lookup entity for payment category
    /// </summary>
    public class Category : CommonNamedEntity
    {
        [Key]
        public virtual int CategoryId { get; set; }

        [NotMapped]
        public override int Id => CategoryId;
        public virtual ICollection<Detail> Details { get; set; }
    }
}
