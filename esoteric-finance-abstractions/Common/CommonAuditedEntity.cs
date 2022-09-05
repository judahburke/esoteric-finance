using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.Common
{
    public abstract class CommonAuditedEntity
    {
        public virtual DateTimeOffset CreatedOn { get; set; }
        public virtual string CreatedBy { get; set; }
    }
}
