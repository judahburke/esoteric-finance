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
    internal class TransactionDetailEntityTypeConfiguration
        : CommonAuditedEntityTypeConfiguration<TransactionDetail>
    {
        public override void Configure(EntityTypeBuilder<TransactionDetail> builder)
        {
            builder
                .Property(p => p.Multiplier)
                .HasConversion<float>()
                .IsRequired();

            builder
                .ToTable(Tables.TransactionDetail, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
