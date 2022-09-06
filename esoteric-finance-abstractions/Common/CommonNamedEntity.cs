using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Esoteric.Finance.Abstractions.Common
{
    public abstract class CommonNamedEntity : CommonAuditedEntity
    {
        [NotMapped]
        public abstract int Id { get; }
        public virtual string Name { get; set; }
    }
}
