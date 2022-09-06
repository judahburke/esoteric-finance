using Esoteric.Finance.Abstractions.Constants.Names;
using Esoteric.Finance.Abstractions.Entities.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoteric.Finance.Data.Configurations.Entities
{
    internal class GeneralLogEntityTypeConfiguration
        : CommonAuditedEntityTypeConfiguration<GeneralLog>
    {
        public override void Configure(EntityTypeBuilder<GeneralLog> builder)
        {
            builder
                .Property(e => e.Category)
                .HasMaxLength(100);
            builder
                .Property(e => e.Scope)
                .HasMaxLength(1000); //todo set this to json
            builder
                .Property(e => e.Message)
                .HasMaxLength(250);
            builder
                .Property(e => e.Exception)
                .HasMaxLength(1000);
            builder
                .ToTable(Tables.GeneralLog, Schemas.dbo);

            base.Configure(builder);
        }
    }
}
