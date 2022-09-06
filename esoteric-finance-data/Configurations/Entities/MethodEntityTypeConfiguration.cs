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
    internal class MethodEntityTypeConfiguration
        : CommonNamedEntityTypeConfiguration<Method>
    {
        public override void Configure(EntityTypeBuilder<Method> builder)
        {
            builder
                .ToTable(Tables.Method, Schemas.Payment);

            base.Configure(builder);
        }
    }
}
