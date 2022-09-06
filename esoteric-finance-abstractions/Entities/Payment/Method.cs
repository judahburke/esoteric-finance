using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Payment
{
    /// <summary>
    /// lookup entity for payment method (sources)
    /// </summary>
    public class Method : CommonNamedEntity
    {
        [Key]
        public virtual int MethodId { get; set; }

        [NotMapped]
        public override int Id => MethodId;
        public virtual ICollection<TransactionMethod> TransactionMethods { get; set; }
    }
}
