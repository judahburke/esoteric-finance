using Esoteric.Finance.Abstractions.Common;
using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.Constants.Names;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esoteric.Finance.Data.Configurations
{
    public abstract class CommonAuditedEntityTypeConfiguration<T>
        : IEntityTypeConfiguration<T> where T : CommonAuditedEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(p => p.CreatedOn)
                .HasConversion(
                    convertToProviderExpression: d => d.UtcDateTime,
                    convertFromProviderExpression: d => new DateTimeOffset(d, TimeSpan.Zero))
                .HasDefaultValueSql(Sqlite.CURRENT_TIMESTAMP);
            builder
                .Property(p => p.CreatedBy)
                .HasMaxLength(100)
                .HasDefaultValue(Apps.APP_NAME);
        }
    }
}
