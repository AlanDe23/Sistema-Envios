using Envios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Infrastructure.Persistence.Configurations
{
    public class BalanceAdminConfiguration : IEntityTypeConfiguration<BalanceAdmin>
    {
        public void Configure(EntityTypeBuilder<BalanceAdmin> builder)
        {
            builder.ToTable("BalanceAdmin");

            builder.HasKey(b => b.IdBalance);

            builder.Property(b => b.TotalMontoPedidos)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.TotalEntregados)
                .HasColumnType("decimal(18,2)");

        }
    }
}
