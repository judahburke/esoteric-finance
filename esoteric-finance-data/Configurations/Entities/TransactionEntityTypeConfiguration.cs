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
    internal class TransactionEntityTypeConfiguration
        : CommonAuditedEntityTypeConfiguration<Transaction>
    {
        public override void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
                .Property(p => p.TransactionDate)
                .HasConversion(
                    convertToProviderExpression: d => d.UtcDateTime, 
                    convertFromProviderExpression: d => new DateTimeOffset(d, TimeSpan.Zero))
                .IsRequired();

            builder
                .Property(p => p.PostedDate)
                .HasConversion(
                    convertToProviderExpression: d => d == null ? (DateTime?)null : d.Value.UtcDateTime,
                    convertFromProviderExpression: d => d == null ? (DateTimeOffset?)null : new DateTimeOffset(d.Value, TimeSpan.Zero));

            builder
                .ToTable(Tables.Transaction, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
