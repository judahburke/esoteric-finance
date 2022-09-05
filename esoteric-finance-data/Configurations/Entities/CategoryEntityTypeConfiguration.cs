using Esoteric.Finance.Abstractions.Constants.Names;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esoteric.Finance.Data.Configurations.Entities
{
    internal class CategoryEntityTypeConfiguration
        : CommonNamedEntityTypeConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .ToTable(Tables.Category, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
