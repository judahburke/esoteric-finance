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
    internal class TransactionSubCategoryEntityTypeConfiguration
        : CommonAuditedEntityTypeConfiguration<TransactionSubCategory>
    {
        public override void Configure(EntityTypeBuilder<TransactionSubCategory> builder)
        {
            builder
                .Property(b => b.Multiplier)
                .IsRequired()
                .HasConversion<double>();

            builder
                .ToTable(Tables.TransactionSubCategory, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
