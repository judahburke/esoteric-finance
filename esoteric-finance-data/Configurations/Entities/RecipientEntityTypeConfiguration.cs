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
    internal class RecipientEntityTypeConfiguration
        : CommonNamedEntityTypeConfiguration<Recipient>
    {
        public override void Configure(EntityTypeBuilder<Recipient> builder)
        {
            builder
                .ToTable(Tables.Recipient, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
