using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// lookup entity for payment recipient (recipients)
    /// one-to-many with <see cref="Transaction"/>
    /// </summary>
    public class Recipient : CommonNamedEntity
    {
        [Key]
        public virtual int RecipientId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
