using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// lookup entity for payment initiator
    /// </summary>
    public class Initiator : CommonNamedEntity
    {
        [Key]
        public virtual int InitiatorId {get; set;}

        [NotMapped]
        public override int Id => InitiatorId;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
