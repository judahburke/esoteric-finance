using Esoteric.Finance.Abstractions.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Esoteric.Finance.Abstractions.Entities.Dbo
{
    public class GeneralLog : CommonAuditedEntity
    {
        [Key]
        public virtual long GeneralLogId { get; set; }
        public virtual int EventCode { get; set; }
        public virtual int LevelCode { get; set; }
        public virtual string Category { get; set; }
        public virtual string? Scope { get; set; }
        public virtual string Message { get; set; }
        public virtual string? Exception { get; set; }
    }
}
