using Esoteric.Finance.Abstractions.Constants.Names;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoteric.Finance.Data.Configurations.Entities
{
    internal class DetailEntityTypeConfiguration
        : CommonAuditedEntityTypeConfiguration<Detail>
    {
        public override void Configure(EntityTypeBuilder<Detail> builder)
        {
            builder
                .Property(p => p.Description)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .ToTable(Tables.Detail, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
