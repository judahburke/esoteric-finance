using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// lookup entity for payment category (genre)
    /// one-to-many with <see cref="SubCategory"/>
    /// </summary>
    public class Category : CommonNamedEntity
    {
        [Key]
        public virtual int CategoryId { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
