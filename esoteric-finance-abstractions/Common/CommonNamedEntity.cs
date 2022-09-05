using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.Common
{
    public abstract class CommonNamedEntity : CommonAuditedEntity
    {
        public virtual string Name { get; set; }
    }
}
