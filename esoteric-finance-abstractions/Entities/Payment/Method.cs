using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public virtual ICollection<TransactionMethod> Transactions { get; set; }
    }
}
