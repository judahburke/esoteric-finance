using Esoteric.Finance.Abstractions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoteric.Finance.Data.Configurations
{
    public abstract class CommonNamedEntityTypeConfiguration<T>
        : CommonAuditedEntityTypeConfiguration<T> where T : CommonNamedEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(p => p.Name)
                .HasMaxLength(250)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
