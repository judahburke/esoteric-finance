using Esoteric.Finance.Abstractions.Constants.Names;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoteric.Finance.Data.Configurations.Entities
{
    internal class TransactionMethodEntityTypeConfiguration
        : CommonAuditedEntityTypeConfiguration<TransactionMethod>
    {
        public override void Configure(EntityTypeBuilder<TransactionMethod> builder)
        {
            builder
                .Property(b => b.Amount)
                .IsRequired()
                .HasConversion<double>();

            builder
                .ToTable(Tables.TransactionMethod, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
